using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Features.Tvshow.Vms;

namespace Tvshow.Application.Features.Tvshow.Commands.CreateTvShow
{
    public class CreateTvShowCommand : IRequest<TvShowVm>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Favorite { get; set; }
    }
}
