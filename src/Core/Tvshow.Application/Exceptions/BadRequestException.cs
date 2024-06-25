using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvshow.Application.Exceptions
{
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string message) : base(message)
        {
            // Constructor que inicializa la excepción con un mensaje de error específico.
            // Llama al constructor base de ApplicationException para establecer el mensaje de la excepción
        }
    }
}
