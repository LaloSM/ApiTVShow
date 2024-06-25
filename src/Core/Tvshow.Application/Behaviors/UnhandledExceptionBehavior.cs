using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvshow.Application.Behaviors
{
    public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        // Constructor que recibe un ILogger<TRequest> para la gestión de registros de eventos
        public UnhandledExceptionBehavior(ILogger<TRequest> logger)
        {
            _logger = logger; // Asigna el logger recibido al campo privado _logger
        }

        // Método Handle que implementa la interfaz IPipelineBehavior<TRequest, TResponse>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next(); // Ejecuta el siguiente manejador en la cadena de responsabilidad
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción que ocurra durante la ejecución del request
                var requestName = typeof(TRequest).Name; // Obtiene el nombre del tipo de TRequest
                _logger.LogError(ex, "Application Request: Ocurrió una excepción para el request {Name} {@Request}", requestName, request);

                // Lanza una nueva excepción encapsulando el mensaje original
                throw new Exception("Application Request con Errores");
            }
        }
    }
}
