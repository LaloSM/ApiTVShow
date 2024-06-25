using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvshow.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key) : base($"Entity \"{name}\" ({name}\" ({key}) no fue encontrado")
        {
            // Constructor que inicializa la excepción con un mensaje de error detallado.
            // Utiliza el constructor base de ApplicationException para establecer el mensaje de la excepción,
            // indicando qué entidad no pudo encontrarse y cuál era su clave (key).
        }
    }
}
