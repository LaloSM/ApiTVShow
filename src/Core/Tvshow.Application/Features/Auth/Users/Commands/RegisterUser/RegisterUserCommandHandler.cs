using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Contracts.Identity;
using Tvshow.Application.Features.Auth.Users.Vms;
using Tvshow.Domain;
using Tvshow.Application.Exceptions;

namespace Tvshow.Application.Features.Auth.Users.Commands.RegisterUser
{
    // Define un controlador de comando para registrar usuarios, implementando IRequestHandler para RegisterUserCommand y devolviendo AuthResponse.
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
    {
        private readonly UserManager<Usuario> _userManager;   // Gestor de usuarios que proporciona operaciones CRUD para usuarios.
        private readonly RoleManager<IdentityRole> _roleManager; // Gestor de roles que gestiona roles de usuario.
        private readonly IAuthService _authService;            // Servicio de autenticación que maneja la generación de tokens JWT.

        // Constructor que inicializa las dependencias necesarias para el handler de RegisterUserCommand.
        public RegisterUserCommandHandler(
            UserManager<Usuario> userManager,
            RoleManager<IdentityRole> roleManager,
            IAuthService authService)
        {
            _userManager = userManager;         // Asigna el gestor de usuarios recibido por inyección de dependencias.
            _roleManager = roleManager;         // Asigna el gestor de roles recibido por inyección de dependencias.
            _authService = authService;         // Asigna el servicio de autenticación recibido por inyección de dependencias.
        }

        // Método que maneja el comando de registro de usuario (RegisterUserCommand).
        public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // Verifica si ya existe un usuario con el mismo email en la base de datos.
            var existeUserByEmail = await _userManager.FindByEmailAsync(request.Email!) is not null;
            if (existeUserByEmail)
            {
                throw new BadRequestException("El email del usuario ya existe en la base de datos"); // Lanza una excepción si el email ya está registrado.
            }

            // Verifica si ya existe un usuario con el mismo UserName en la base de datos.
            var existeUserByUserName = await _userManager.FindByNameAsync(request.UserName!) is not null;
            if (existeUserByUserName)
            {
                throw new BadRequestException("El UserName del usuario ya existe en la base de datos"); // Lanza una excepción si el UserName ya está registrado.
            }

            // Crea una nueva instancia de Usuario con los datos proporcionados en el comando de registro.
            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Telefono = request.Telefono,
                Email = request.Email,
                UserName = request.UserName,
                IsActive = true
            };

            // Intenta crear el usuario en la base de datos utilizando el gestor de usuarios.
            var resultado = await _userManager.CreateAsync(usuario, request.Password!);

            if (resultado.Succeeded)
            {
                // Si el usuario se crea exitosamente, se le asigna el rol de usuario genérico y se obtienen los roles del usuario.
                await _userManager.AddToRoleAsync(usuario, AppRole.GenericUser);
                var roles = await _userManager.GetRolesAsync(usuario);

                // Construye y devuelve una respuesta de autenticación con los datos del usuario y el token generado por el servicio de autenticación.
                return new AuthResponse
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Telefono = usuario.Telefono,
                    Email = usuario.Email,
                    UserName = usuario.UserName,
                    Token = _authService.CreateToken(usuario, roles),
                    Roles = roles
                };
            }

            // Si no se puede registrar el usuario, lanza una excepción general.
            throw new Exception("No se pudo registrar el usuario");
        }
    }
}
