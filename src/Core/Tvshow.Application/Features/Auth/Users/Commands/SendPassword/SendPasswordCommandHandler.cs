using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Contracts.Infrastructure;
using Tvshow.Application.Models.Email;
using Tvshow.Domain;
using Tvshow.Application.Exceptions;


namespace Tvshow.Application.Features.Auth.Users.Commands.SendPassword
{
    // Clase que maneja el comando para enviar correo electrónico para restablecer contraseña (SendPasswordCommand).
    public class SendPasswordCommandHandler : IRequestHandler<SendPasswordCommand, string>
    {
        // Servicios inyectados necesarios.
        private readonly IEmailService _serviceEmail; // Servicio para enviar correos electrónicos.
        private readonly UserManager<Usuario> _userManager; // Gestor de usuarios para acceder a funcionalidades de usuario.

        // Constructor que inicializa el manejador con los servicios necesarios.
        public SendPasswordCommandHandler(IEmailService serviceEmail, UserManager<Usuario> userManager)
        {
            _serviceEmail = serviceEmail; // Inicializa el servicio de correo electrónico.
            _userManager = userManager; // Inicializa el gestor de usuarios.
        }

        // Método que maneja la lógica para enviar el correo electrónico de restablecimiento de contraseña.
        public async Task<string> Handle(SendPasswordCommand request, CancellationToken cancellationToken)
        {
            // Busca al usuario por su correo electrónico en la base de datos.
            var usuario = await _userManager.FindByEmailAsync(request.Email!);

            // Si el usuario no existe, lanza una excepción BadRequestException.
            if (usuario == null)
            {
                throw new BadRequestException("El usuario no existe");
            }

            // Genera un token para restablecer la contraseña del usuario.
            var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);

            // Convierte el token en bytes y luego a base64 para incluirlo en el email.
            var plainTextBytes = Encoding.UTF8.GetBytes(token);
            token = Convert.ToBase64String(plainTextBytes);

            // Prepara el mensaje de correo electrónico.
            var emailMessage = new EmailMessage
            {
                To = request.Email, // Destinatario del correo (email del usuario).
                Body = "Para restablecer tu contraseña, haz clic en el siguiente enlace:", // Cuerpo del correo.
                Subject = "Restablecimiento de Contraseña" // Asunto del correo.
            };

            // Envía el correo electrónico utilizando el servicio de email con el mensaje y el token.
            var result = await _serviceEmail.SendEmail(emailMessage, token);

            // Si no se pudo enviar el correo electrónico, lanza una excepción genérica.
            if (!result)
            {
                throw new Exception("No se pudo enviar el email");
            }

            // Retorna un mensaje indicando que se envió el correo satisfactoriamente.
            return $"Se envió el email a la cuenta {request.Email}";
        }
    }
}
