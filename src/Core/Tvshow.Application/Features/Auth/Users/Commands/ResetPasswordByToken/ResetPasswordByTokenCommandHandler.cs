using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Domain;
using Tvshow.Application.Exceptions;

namespace Tvshow.Application.Features.Auth.Users.Commands.ResetPasswordByToken
{
    public class ResetPasswordByTokenCommandHandler : IRequestHandler<ResetPasswordByTokenCommand, string>
    {
        private readonly UserManager<Usuario> _userManager;

        // Constructor que inicializa el controlador de comando con el gestor de usuarios proporcionado.
        public ResetPasswordByTokenCommandHandler(UserManager<Usuario> userManager)
        {
            _userManager = userManager; // Asigna el gestor de usuarios recibido por inyección de dependencias.
        }

        // Método que maneja el comando de restablecimiento de contraseña por token (ResetPasswordByTokenCommand).
        public async Task<string> Handle(ResetPasswordByTokenCommand request, CancellationToken cancellationToken)
        {
            // Verifica que la contraseña y la confirmación de la contraseña sean iguales.
            if (!string.Equals(request.Password, request.ConfirmPassword))
            {
                throw new BadRequestException("El password no es igual a la confirmacion del password");
            }

            // Busca al usuario por su correo electrónico en la base de datos.
            var updateUsuario = await _userManager.FindByEmailAsync(request.Email!);

            // Si el usuario no se encuentra registrado, lanza una excepción.
            if (updateUsuario is null)
            {
                throw new BadRequestException("El email no esta registrado como usuario");
            }

            // Convierte el token de base64 recibido en una cadena de texto UTF-8.
            var token = Convert.FromBase64String(request.Token!);
            var tokenResult = Encoding.UTF8.GetString(token);

            // Intenta restablecer la contraseña del usuario usando el token y la nueva contraseña proporcionada.
            var resultResultado = await _userManager.ResetPasswordAsync(updateUsuario, tokenResult, request.Password!);

            // Si el restablecimiento de contraseña no tiene éxito, lanza una excepción.
            if (!resultResultado.Succeeded)
            {
                throw new Exception("No se pudo resetear el password");
            }

            // Retorna un mensaje de éxito indicando que la contraseña se ha actualizado correctamente.
            return $"Se actualizo exitosamente tu password ${request.Email}";
        }
    }
}
