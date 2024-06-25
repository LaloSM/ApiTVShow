using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Features.Tvshow.Vms;
using Tvshow.Application.Persistence;
using Tvshow.Domain;

namespace Tvshow.Application.Features.Tvshow.Queries.GetTvShowList
{
    public class GetTvShowListQueryHandler : IRequestHandler<GetTvShowListQuery, IReadOnlyList<TvShowVm>>
    {
        private readonly IUnitOfWork _unitoWork;
        private readonly IMapper _mapper;

        public GetTvShowListQueryHandler(IUnitOfWork unitoWork, IMapper mapper)
        {
            _unitoWork = unitoWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<TvShowVm>> Handle(GetTvShowListQuery request, CancellationToken cancellationToken)
        {
            var tvShows = await _unitoWork.Repository<TvShow>().GetAsync(
                null, // Filtro: null indica que no se aplican filtros adicionales, se recuperan todos los programas de televisión.
                x => x.OrderBy(y => y.Name), // Ordenación: ordena los resultados por el nombre del programa de televisión.
                string.Empty, // Inclusión de entidades relacionadas: cadena vacía indica que no se incluirán entidades relacionadas.
                false // No rastrear entidades cargadas para mejorar el rendimiento.
            );

            return _mapper.Map<IReadOnlyList<TvShowVm>>(tvShows); // Mapeo: convierte la lista de entidades TvShow en TvShowVm.
        }

    }
}
