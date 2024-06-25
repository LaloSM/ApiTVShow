using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Features.Auth.Users.Vms;

namespace Tvshow.Application.Features.Auth.Users.Queries.GetUserById
{
    // Clase que representa una consulta para obtener un usuario por su ID.
    public class GetUserByIdQuery : IRequest<AuthResponse>
    {
        public string? UserId { get; set; } // Propiedad que almacena el ID del usuario a consultar.

        // Constructor que inicializa la consulta con el ID del usuario.
        public GetUserByIdQuery(string? userId)
        {
            UserId = userId ?? throw new ArgumentException(nameof(userId)); // Asigna el ID del usuario asegurándose de que no sea nulo.
        }
    }
}
