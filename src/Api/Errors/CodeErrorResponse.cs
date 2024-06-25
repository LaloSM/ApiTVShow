using Newtonsoft.Json;

namespace Tvshow.Api.Errors
{
    public class CodeErrorResponse
    {
        [JsonProperty(PropertyName = "statusCode")]
        public int StatusCode { get; set; }  // Propiedad que representa el código de estado HTTP

        [JsonProperty(PropertyName = "message")]
        public string[]? Message { get; set; }  // Propiedad que representa el mensaje de error o mensajes de error

        // Constructor de la clase CodeErrorResponse
        public CodeErrorResponse(int statusCode, string[]? message = null)
        {
            StatusCode = statusCode;  // Asigna el código de estado proporcionado al objeto CodeErrorResponse

            if (message is null)
            {
                Message = new string[0];  // Crea un arreglo de cadenas vacío si el mensaje es nulo
                var text = GetDefaultMessageStatusCode(statusCode);  // Obtiene un mensaje predeterminado según el código de estado
                Message[0] = text;  // Asigna el mensaje predeterminado al primer elemento del arreglo de mensajes
            }
            else
            {
                Message = message;  // Asigna el mensaje proporcionado al arreglo de mensajes
            }
        }

        // Método privado que devuelve un mensaje predeterminado según el código de estado
        private string GetDefaultMessageStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "El Request enviado tiene errores",  // Mensaje para el código de estado 400 (Bad Request)
                401 => "No tienes autorización para este recurso",  // Mensaje para el código de estado 401 (Unauthorized)
                404 => "No se encontró el recurso solicitado",  // Mensaje para el código de estado 404 (Not Found)
                500 => "Se produjeron errores en el servidor",  // Mensaje para el código de estado 500 (Internal Server Error)
                _ => string.Empty  // Mensaje por defecto para otros códigos de estado
            };
        }
    }

}
