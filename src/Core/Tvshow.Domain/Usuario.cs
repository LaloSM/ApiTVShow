using Microsoft.AspNetCore.Identity;

namespace Tvshow.Domain
{
    public class Usuario : IdentityUser {
        //Estos son datos adicionales del identity
        public string? Nombre { get; set; }

        public string? Apellido { get; set; }

        public string? Telefono { get; set; }

        public bool IsActive { get; set; }

    }
}
