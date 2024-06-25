using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Features.Auth.Users.Vms;

namespace Tvshow.Application.Features.Auth.Users.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<AuthResponse>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
