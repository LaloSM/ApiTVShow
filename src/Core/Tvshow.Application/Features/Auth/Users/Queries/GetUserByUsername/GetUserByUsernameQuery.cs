using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Features.Auth.Users.Vms;

namespace Tvshow.Application.Features.Auth.Users.Queries.GetUserByUsername
{
    public class GetUserByUsernameQuery : IRequest<AuthResponse>
    {
        public string? UserName { get; set; }

        public GetUserByUsernameQuery(string userName)
        {
            UserName = userName ?? throw new ArgumentException(nameof(UserName));
        }
    }
}
