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
using Tvshow.Application.Exceptions;

namespace Tvshow.Application.Features.Auth.Users.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponse>
    {
        // Declaraciones de variables privadas para la gestión de usuarios, roles, autenticación, mapeo y unidad de trabajo.
        private readonly UserManager<Usuario> _userManager;   // Gestor de usuarios que proporciona operaciones CRUD para usuarios.
        private SignInManager<Usuario> _signInManager;         // Gestor de inicio de sesión que facilita las operaciones de inicio de sesión de usuarios.
        private readonly RoleManager<IdentityRole> _roleManager; // Gestor de roles que gestiona roles de usuario.
        private readonly IAuthService _authService;            // Servicio de autenticación que maneja la generación de tokens JWT.
        private readonly IMapper _mapper;                      // Biblioteca de mapeo que facilita la conversión entre objetos.
        private readonly IUnitOfWork _unitOfWork;              // Unidad de trabajo que coordina las operaciones con la base de datos.

        // Constructor que inicializa las dependencias necesarias para el handler de LoginUserCommand.
        public LoginUserCommandHandler(UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager,
            RoleManager<IdentityRole> roleManager,
            IAuthService authService,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;         // Asigna el gestor de usuarios recibido por inyección de dependencias.
            _signInManager = signInManager;     // Asigna el gestor de inicio de sesión recibido por inyección de dependencias.
            _roleManager = roleManager;         // Asigna el gestor de roles recibido por inyección de dependencias.
            _authService = authService;         // Asigna el servicio de autenticación recibido por inyección de dependencias.
            _mapper = mapper;                   // Asigna el mapeador recibido por inyección de dependencias.
            _unitOfWork = unitOfWork;           // Asigna la unidad de trabajo recibida por inyección de dependencias.
        }

        // Método que maneja el comando de inicio de sesión (LoginUserCommand).
        public async Task<AuthResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // Busca un usuario por su dirección de correo electrónico.
            var user = await _userManager.FindByEmailAsync(request.Email!);
            if (user is null)
            {
                throw new NotFoundException(nameof(Usuario), request.Email!); // Lanza una excepción si el usuario no se encuentra.
            }

            // Verifica la validez de las credenciales del usuario utilizando el gestor de inicio de sesión.
            var resultado = await _signInManager.CheckPasswordSignInAsync(user, request.Password!, false);

            if (!resultado.Succeeded)
            {
                throw new Exception("Las credenciales del usuario son incorrectas"); // Lanza una excepción si las credenciales son incorrectas.
            }

            // Obtiene los roles del usuario utilizando el gestor de usuarios.
            var roles = await _userManager.GetRolesAsync(user);

            // Construye una respuesta de autenticación con los datos del usuario y el token generado por el servicio de autenticación.
            var authResponse = new AuthResponse
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Telefono = user.Telefono,
                Email = user.Email,
                UserName = user.UserName,
                Token = _authService.CreateToken(user, roles), // Genera un token JWT para el usuario autenticado.
                Roles = roles                                 // Asigna los roles del usuario a la respuesta de autenticación.
            };

            return authResponse; // Devuelve la respuesta de autenticación.
        }
    }
}
