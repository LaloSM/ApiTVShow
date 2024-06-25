using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Contracts.Identity;
using Tvshow.Domain;
using Tvshow.Application.Exceptions;

namespace Tvshow.Application.Features.Auth.Users.Commands.ResetPassword
{
    // Define un controlador de comando para restablecer la contraseña, implementando IRequestHandler para ResetPasswordCommand.
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly UserManager<Usuario> _userManager;  // Gestor de usuarios que proporciona operaciones CRUD para la entidad Usuario.
        private readonly IAuthService _authService;          // Servicio de autenticación que proporciona métodos para la gestión de sesiones de usuario.

        // Constructor que inicializa las dependencias necesarias para el handler de ResetPasswordCommand.
        public ResetPasswordCommandHandler(UserManager<Usuario> userManager, IAuthService authService)
        {
            _userManager = userManager;     // Asigna el gestor de usuarios recibido por inyección de dependencias.
            _authService = authService;     // Asigna el servicio de autenticación recibido por inyección de dependencias.
        }

        // Método que maneja el comando de restablecimiento de contraseña (ResetPasswordCommand).
        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // Obtiene el usuario a actualizar usando el nombre de usuario almacenado en la sesión actual.
            var updateUsuario = await _userManager.FindByNameAsync(_authService.GetSessionUser());

            if (updateUsuario is null)
            {
                throw new BadRequestException("El usuario no existe"); // Lanza una excepción si el usuario no se encuentra en la base de datos.
            }

            // Verifica que la contraseña actual proporcionada coincida con la contraseña almacenada del usuario.
            var resultValidateOldPassword = _userManager.PasswordHasher.VerifyHashedPassword(updateUsuario, updateUsuario.PasswordHash!, request.OldPassword!);
            if (!(resultValidateOldPassword == PasswordVerificationResult.Success))
            {
                throw new BadRequestException("El actual password ingresado es erroneo"); // Lanza una excepción si la contraseña actual no coincide.
            }

            // Encripta la nueva contraseña proporcionada por el usuario.
            var hashedNewPassword = _userManager.PasswordHasher.HashPassword(updateUsuario, request.NewPassword!);
            updateUsuario.PasswordHash = hashedNewPassword;

            // Intenta actualizar la contraseña del usuario en la base de datos usando el gestor de usuarios.
            var resultado = await _userManager.UpdateAsync(updateUsuario);

            if (!resultado.Succeeded)
            {
                throw new Exception("No se pudo resetear el password"); // Lanza una excepción general si no se puede actualizar la contraseña.
            }

            return Unit.Value; // Devuelve Unit.Value para indicar que el proceso de restablecimiento de contraseña se completó con éxito.
        }
    }

}
