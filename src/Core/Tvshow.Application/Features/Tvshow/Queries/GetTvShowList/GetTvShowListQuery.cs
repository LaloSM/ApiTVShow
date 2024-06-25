using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Features.Tvshow.Vms;

namespace Tvshow.Application.Features.Tvshow.Queries.GetTvShowList
{
    public class GetTvShowListQuery : IRequest<IReadOnlyList<TvShowVm>>
    {
    }
}
