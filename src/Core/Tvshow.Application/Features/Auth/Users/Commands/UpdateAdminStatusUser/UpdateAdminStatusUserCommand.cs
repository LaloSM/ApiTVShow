using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Domain;

namespace Tvshow.Application.Features.Auth.Users.Commands.UpdateAdminStatusUser
{
    public class UpdateAdminStatusUserCommand : IRequest<Usuario>
    {
        public string? Id { get; set; }
    }
}
