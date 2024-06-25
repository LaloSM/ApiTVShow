using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Domain;
using Tvshow.Application.Exceptions;

namespace Tvshow.Application.Features.Auth.Users.Commands.UpdateAdminStatusUser
{
    // Clase que maneja el comando para actualizar el estado de activación de un usuario administrativo.
    public class UpdateAdminStatusUserCommandHandler : IRequestHandler<UpdateAdminStatusUserCommand, Usuario>
    {
        private readonly UserManager<Usuario> _userManager; // Gestor de usuarios para acceder a funcionalidades de usuario.

        // Constructor que inicializa el manejador con el gestor de usuarios.
        public UpdateAdminStatusUserCommandHandler(UserManager<Usuario> userManager)
        {
            _userManager = userManager; // Inicializa el gestor de usuarios.
        }

        // Método que maneja la lógica para actualizar el estado de activación del usuario.
        public async Task<Usuario> Handle(UpdateAdminStatusUserCommand request, CancellationToken cancellationToken)
        {
            // Busca al usuario por su Id en la base de datos.
            var updateUsuario = await _userManager.FindByIdAsync(request.Id!);

            // Si el usuario no existe, lanza una excepción BadRequestException.
            if (updateUsuario is null)
            {
                throw new BadRequestException("El usuario no existe");
            }

            // Invierte el estado de activación del usuario.
            updateUsuario.IsActive = !updateUsuario.IsActive;

            // Actualiza el usuario en la base de datos.
            var resultado = await _userManager.UpdateAsync(updateUsuario);

            // Si no se pudo actualizar el usuario, lanza una excepción genérica.
            if (!resultado.Succeeded)
            {
                throw new Exception("No se pudo cambiar el estado del usuario");
            }

            // Retorna el usuario actualizado.
            return updateUsuario;
        }
    }

}
