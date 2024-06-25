using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Features.Tvshow.Vms;

namespace Tvshow.Application.Features.Tvshow.Commands.DeleteTvShow
{
    public class DeleteTvShowCommand : IRequest<TvShowVm>
    {
        public int IdTvShow { get; set; }
        public DeleteTvShowCommand(int idTvShow)
        {
            IdTvShow = idTvShow == 0 ? throw new ArgumentException(nameof(idTvShow)) : idTvShow;
        }
    }
}
