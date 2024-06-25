using MailKit.Search;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Features.Tvshow.Vms;

namespace Tvshow.Application.Features.Tvshow.Queries.GetTvShowById
{
    public class GetTvShowByIdQuery : IRequest<TvShowVm>
    {
        public int Id { get; set; }

        public GetTvShowByIdQuery(int id)
        {
            Id = id == 0 ? throw new ArgumentNullException(nameof(id)) : id;
        }
    }
}
