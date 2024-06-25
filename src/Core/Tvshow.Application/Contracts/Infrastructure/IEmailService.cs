using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Models.Email;

namespace Tvshow.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        // Método para enviar un correo electrónico utilizando SendGrid.
        // Se encarga de enviar un mensaje de correo electrónico usando las credenciales proporcionadas
        // en el parámetro 'token' y la información del correo electrónico en 'email'.
        // Retorna un booleano indicando si el correo se envió exitosamente.
        Task<bool> SendEmail(EmailMessage email, string token);
    }
}
