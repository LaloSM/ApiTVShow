using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvshow.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        // Constructor que recibe una colección de validadores para TRequest
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators; // Asigna los validadores recibidos al campo privado _validators
        }

        // Método Handle que implementa la interfaz IPipelineBehavior<TRequest, TResponse>
        public async Task<TResponse> Handle(
                                    TRequest request,
                                    RequestHandlerDelegate<TResponse> next,
                                    CancellationToken cancellationToken)
        {
            // Verifica si existen validadores registrados para TRequest
            if (_validators.Any())
            {
                // Crea un contexto de validación para TRequest
                var context = new ValidationContext<TRequest>(request);

                // Ejecuta la validación asincrónica para cada validador
                var validatorResult = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                // Combina todos los errores de validación en una lista
                var failures = validatorResult.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                // Si hay errores de validación, lanza una excepción de validación
                if (failures.Count != 0)
                {
                    throw new ValidationException(failures);
                }
            }

            // Si no hay errores de validación, ejecuta el siguiente manejador en la cadena de responsabilidad
            return await next();
        }
    }
}
