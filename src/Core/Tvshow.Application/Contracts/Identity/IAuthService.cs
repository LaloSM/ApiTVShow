using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Domain;

namespace Tvshow.Application.Contracts.Identity
{
    public interface IAuthService
    {
        // Método para obtener el usuario de la sesión actual
        string GetSessionUser();

        // Método para crear un token de autenticación basado en el usuario y roles proporcionados
        string CreateToken(Usuario usuario, IList<string>? roles);

    }
}
