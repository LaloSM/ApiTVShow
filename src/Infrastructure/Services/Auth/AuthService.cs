using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Contracts.Identity;
using Tvshow.Application.Models.Token;
using Tvshow.Domain;

namespace Tvshow.Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        public JwtSettings _jwtSettings { get; }
        // JwtSettings es una clase que contiene la configuración relacionada con JWT.
        // Esta propiedad almacena la configuración de JWT obtenida de la capa de aplicación.

        private readonly IHttpContextAccessor _httpContextAccesor;
        // IHttpContextAccessor proporciona acceso al contexto HTTP actual.
        // Se utiliza para obtener información del request del cliente.

        public AuthService(IHttpContextAccessor httpContextAccessor, IOptions<JwtSettings> jwtSittings)
        {
            _httpContextAccesor = httpContextAccessor;
            _jwtSettings = jwtSittings.Value;
        }
        // Constructor de la clase AuthService que recibe IHttpContextAccessor y IOptions<JwtSettings>.
        // IHttpContextAccessor se utiliza para acceder al contexto HTTP actual.
        // IOptions<JwtSettings> se utiliza para inyectar la configuración de JwtSettings desde la capa de configuración.

        // Método para crear un token JWT.
        public string CreateToken(Usuario usuario, IList<string>? roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName!),
                new Claim("userId", usuario.Id),
                new Claim("email", usuario.Email!)
            };

            // Agrega los roles como claims al token.
            foreach (var rol in roles!)
            {
                var claim = new Claim(ClaimTypes.Role, rol);
                claims.Add(claim);
            }

            // Obtiene la clave secreta para firmar el token.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key!));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            // Describe las características del token JWT a crear.
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.ExpireTime), // Define la expiración del token.
                SigningCredentials = credenciales // Asigna las credenciales para firmar el token.
            };

            // Crea el token JWT.
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            // Devuelve el token JWT como un string.
            return tokenHandler.WriteToken(token);
        }

        // Método para obtener el nombre de usuario de la sesión actual.
        public string GetSessionUser()
        {
            // Obtiene el nombre de usuario del claim 'NameIdentifier' del contexto HTTP actual.
            var username = _httpContextAccesor.HttpContext!.User?.Claims?
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            return username!;
        }
    }
}
