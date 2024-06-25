using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Features.Tvshow.Vms;

namespace Tvshow.Application.Features.Tvshow.Commands.UpdateTvShow
{
    public class UpdateTvShowCommand : IRequest<TvShowVm>
    {
        public int IdTvShow { get; set; }
        public string Name { get; set; }
        public int Favorite { get; set; }
    }
}
