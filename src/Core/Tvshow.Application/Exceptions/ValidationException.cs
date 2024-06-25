using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvshow.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public IDictionary<string, string[]> Errors { get; }
        // Constructor que inicializa una instancia de ValidationException sin errores específicos.
        public ValidationException() : base("Se presentaron uno o más errores de validación")
        {
            Errors = new Dictionary<string, string[]>(); // Inicializa el diccionario de errores.
        }

        // Constructor que inicializa una instancia de ValidationException con una colección de errores de validación.
        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
            // Agrupa los errores de validación por el nombre de la propiedad y los convierte en un diccionario de
            // cadenas de errores para cada propiedad.
        }
    }
}
