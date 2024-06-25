using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using Tvshow.Api.Errors;
using Tvshow.Application.Exceptions;

namespace Tvshow.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionMiddleware> _logger;

        //Constructor de la clase ExceptionMiddleware que recibe el RequestDelegate y el ILogger
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;  // Inicializa el delegate para invocar el siguiente middleware
            _logger = logger;  // Inicializa el logger para registrar errores
        }

        // Método InvokeAsync para manejar las excepciones de manera asíncrona
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Intenta invocar el siguiente middleware en la pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, se captura aquí
                _logger.LogError(ex, ex.Message);  // Registra el error en el logger con su mensaje y excepción

                context.Response.ContentType = "application/json";  // Establece el tipo de contenido de la respuesta como JSON
                var statusCode = (int)HttpStatusCode.InternalServerError;  // Código de estado por defecto para errores internos
                var result = string.Empty;  // Inicializa una cadena vacía para el resultado JSON de la respuesta

                // Manejo específico de diferentes tipos de excepciones
                switch (ex)
                {
                    case NotFoundException notFoundException:
                        // Si la excepción es del tipo NotFoundException, se establece el código de estado como NotFound
                        statusCode = (int)HttpStatusCode.NotFound;
                        break;

                    case FluentValidation.ValidationException validationException:
                        // Si la excepción es del tipo ValidationException de FluentValidation, se maneja como BadRequest
                        statusCode = (int)HttpStatusCode.BadRequest;
                        var errors = validationException.Errors.Select(error => error.ErrorMessage).ToArray();
                        var validationJson = JsonConvert.SerializeObject(errors);  // Serializa los errores de validación a JSON
                                                                                   // Crea una respuesta de error personalizada utilizando la clase CodeErrorException
                        result = JsonConvert.SerializeObject(new CodeErrorException(statusCode, errors, validationJson));
                        break;

                    case BadRequestException badRequestException:
                        // Si la excepción es del tipo BadRequestException, se establece el código de estado como BadRequest
                        statusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    default:
                        // Para cualquier otra excepción no manejada explícitamente, se usa el código de estado InternalServerError
                        statusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                // Si no se ha establecido un resultado personalizado, se crea uno genérico basado en la excepción general
                if (string.IsNullOrEmpty(result))
                {
                    result = JsonConvert.SerializeObject(new CodeErrorException(statusCode, new string[] { ex.Message }, ex.StackTrace));
                }

                // Establece el código de estado de la respuesta HTTP
                context.Response.StatusCode = statusCode;
                // Escribe el resultado JSON en la respuesta HTTP
                await context.Response.WriteAsync(result);
            }
        }
    }
}
