using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Features.Auth.Users.Vms;
using Tvshow.Domain;

namespace Tvshow.Application.Features.Auth.Users.Queries.GetUserByUsername
{
    // Manejador de consulta para obtener información detallada de un usuario por nombre de usuario.
    public class GetUserByUsernameHandler : IRequestHandler<GetUserByUsernameQuery, AuthResponse>
    {
        private readonly UserManager<Usuario> _userManager; // Gestor de usuarios para interactuar con los datos de usuario.

        // Constructor que inicializa el gestor de usuarios.
        public GetUserByUsernameHandler(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        // Método que maneja la solicitud de la consulta para obtener un usuario por nombre de usuario.
        public async Task<AuthResponse> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName!); // Busca al usuario por su nombre de usuario.
            if (user is null)
            {
                throw new Exception($"El usuario no existe"); // Lanza una excepción si el usuario no fue encontrado.
            }

            var roles = await _userManager.GetRolesAsync(user); // Obtiene los roles del usuario mediante el gestor de usuarios.

            // Retorna un objeto AuthResponse con los detalles del usuario encontrado.
            return new AuthResponse
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Telefono = user.Telefono,
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles // Asigna los roles obtenidos al objeto AuthResponse.
            };
        }
    }

}
