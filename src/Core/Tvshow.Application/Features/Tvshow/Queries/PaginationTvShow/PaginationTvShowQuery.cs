using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Features.Tvshow.Vms;
using Tvshow.Application.Shared.Queries;

namespace Tvshow.Application.Features.Tvshow.Queries.PaginationTvShow
{
    public class PaginationTvShowQuery : PaginationBaseQuery, IRequest<PaginationVm<TvShowVm>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Favorite { get; set; }
    }
}
