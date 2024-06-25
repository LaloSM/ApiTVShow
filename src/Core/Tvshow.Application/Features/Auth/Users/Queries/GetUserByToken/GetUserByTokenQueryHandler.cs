using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Contracts.Identity;
using Tvshow.Application.Features.Auth.Users.Vms;
using Tvshow.Application.Persistence;
using Tvshow.Domain;

namespace Tvshow.Application.Features.Auth.Users.Queries.GetUserByToken
{
    // Manejador de consulta para obtener información detallada de un usuario por token de sesión.
    public class GetUserByTokenQueryHandler : IRequestHandler<GetUserByTokenQuery, AuthResponse>
    {
        private readonly UserManager<Usuario> _userManager; // Gestor de usuarios para interactuar con los datos de usuario.
        private readonly IAuthService _authService; // Servicio de autenticación para obtener el usuario de sesión actual.
        private readonly IUnitOfWork _unitOfWork; // Unidad de trabajo para operaciones transaccionales.
        private readonly IMapper _mapper; // Mapeador de objetos para realizar conversiones entre tipos.

        // Constructor que inicializa las dependencias requeridas.
        public GetUserByTokenQueryHandler(
            UserManager<Usuario> userManager,
            IAuthService authService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _userManager = userManager;
            _authService = authService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Método que maneja la solicitud de la consulta para obtener un usuario por token de sesión.
        public async Task<AuthResponse> Handle(GetUserByTokenQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(_authService.GetSessionUser()); // Busca al usuario por el nombre de usuario de sesión actual obtenido del servicio de autenticación.
            if (user is null)
            {
                throw new Exception("El usuario no está autenticado"); // Lanza una excepción si el usuario no está autenticado o no se encuentra.
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
                Token = _authService.CreateToken(user, roles), // Genera un nuevo token de sesión para el usuario.
                Roles = roles // Asigna los roles obtenidos al objeto AuthResponse.
            };
        }
    }

}
