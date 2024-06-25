using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Features.Auth.Users.Vms;

namespace Tvshow.Application.Features.Auth.Users.Queries.GetUserByToken
{
    public class GetUserByTokenQuery : IRequest<AuthResponse>
    {
        //no pasamos nada porque lo obtenemos desde el headerRequest
    }
}
