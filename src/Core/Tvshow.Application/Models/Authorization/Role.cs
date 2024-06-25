using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvshow.Application.Models.Authorization
{
    public class Role
    {
        // Define la constante pública para el rol de administrador
        public const string ADMIN = nameof(ADMIN);

        // Define la constante pública para el rol de usuario normal
        public const string USER = nameof(USER);
    }
}
