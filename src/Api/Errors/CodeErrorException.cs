using Newtonsoft.Json;

namespace Tvshow.Api.Errors
{
    public class CodeErrorException : CodeErrorResponse
    {
        // Atributo de JsonProperty que especifica el nombre del atributo en JSON como "details"
        [JsonProperty(PropertyName = "details")]
        // Propiedad pública que representa los detalles adicionales de la excepción
        public string? Details { get; set; }

        // Constructor de la clase CodeErrorException que hereda de una clase base con parámetros statusCode y message
        public CodeErrorException(int statusCode, string[]? message = null, string? details = null) : base(statusCode, message)
        {
            // Asigna el valor del parámetro details a la propiedad Details de la clase
            Details = details;
        }

    }
}
