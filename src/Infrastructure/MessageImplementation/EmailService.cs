using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Contracts.Infrastructure;
using Tvshow.Application.Models.Email;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace Tvshow.Infrastructure.MessageImplementation
{
    public class EmailService : IEmailService
    {
        public EmailSettings _emailSettings { get; }
        public ILogger<EmailService> _logger { get; }

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }
        public async Task<bool> SendEmail(EmailMessage email, string token)
        {
            try
            {
                // Crea una nueva instancia del cliente SendGrid utilizando la clave API obtenida de la configuración
                var client = new SendGridClient(_emailSettings.Key);
                // Configura el remitente del correo electrónico utilizando la dirección de email configurada en _emailSettings
                var from = new EmailAddress(_emailSettings.Email);
                // Obtiene el asunto del correo electrónico del objeto 'email' proporcionado como parámetro
                var subject = email.Subject;
                // Configura el destinatario del correo electrónico utilizando la dirección de email proporcionada en el objeto 'email' como destinatario
                var to = new EmailAddress(email.To, email.To);
                // Configura el contenido del correo electrónico en texto plano
                var plaintTextContent = email.Body;
                // Crea el contenido HTML del correo electrónico, que incluye el cuerpo del email y un enlace generado dinámicamente
                var htmlContent = $"{email.Body} {_emailSettings.BaseUrlClient}/password/reset/{token}";
                // Crea un mensaje de correo electrónico utilizando los datos configurados anteriormente
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plaintTextContent, htmlContent);
                // Envía el correo electrónico utilizando el cliente SendGrid y espera la respuesta
                var response = await client.SendEmailAsync(msg);
                // Retorna true si el envío fue exitoso (código de estado HTTP 200-299), de lo contrario, false
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                _logger.LogError("El email no pudo enviarle, existen errores");
                return false;
            }
        }
    }
}
