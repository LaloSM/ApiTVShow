using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Features.Auth.Users.Vms;
using Tvshow.Domain;

namespace Tvshow.Application.Features.Auth.Users.Queries.GetUserById
{
    // Manejador de consulta para obtener información detallada de un usuario por su ID.
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, AuthResponse>
    {
        private readonly UserManager<Usuario> _userManager; // Gestor de usuarios para interactuar con los datos de usuario.

        // Constructor que inicializa el gestor de usuarios.
        public GetUserByIdQueryHandler(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        // Método que maneja la solicitud de la consulta para obtener un usuario por su ID.
        public async Task<AuthResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId!); // Busca al usuario por su ID proporcionado en la solicitud.
            if (user is null)
            {
                throw new Exception("El usuario no existe"); // Lanza una excepción si el usuario no es encontrado.
            }

            // Retorna un objeto AuthResponse con los detalles del usuario encontrado.
            return new AuthResponse
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Telefono = user.Telefono,
                Email = user.Email,
                UserName = user.UserName,
                Roles = await _userManager.GetRolesAsync(user) // Obtiene los roles del usuario mediante el gestor de usuarios.
            };
        }
    }

}
