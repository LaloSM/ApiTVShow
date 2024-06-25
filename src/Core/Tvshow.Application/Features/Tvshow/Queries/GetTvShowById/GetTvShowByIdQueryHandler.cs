using AutoMapper;
using MailKit.Search;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Features.Tvshow.Vms;
using Tvshow.Application.Persistence;
using Tvshow.Domain;

namespace Tvshow.Application.Features.Tvshow.Queries.GetTvShowById
{
    public class GetTvShowByIdQueryHandler : IRequestHandler<GetTvShowByIdQuery, TvShowVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTvShowByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TvShowVm> Handle(GetTvShowByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var includes = new List<Expression<Func<TvShow, object>>>(); // Lista de expresiones de inclusión para incluir entidades relacionadas.

                // Obtener el programa de televisión utilizando el método GetEntityAsync de la unidad de trabajo.
                var tvshow = await _unitOfWork.Repository<TvShow>().GetEntityAsync(
                    x => x.Id == request.Id, // Condición de búsqueda: Id igual al Id solicitado en la consulta.
                    includes, // Expresiones de inclusión para incluir entidades relacionadas si es necesario.
                    false // Indica si se debe rastrear o no las entidades cargadas (en este caso, se establece en false para mejorar el rendimiento).
                );

                // Mapear el resultado a TvShowVm si se encontró el programa de televisión
                return _mapper.Map<TvShowVm>(tvshow);
            }
            catch (Exception ex)
            {
                // Aquí puedes manejar la excepción como prefieras
                // Por ejemplo, puedes registrar el error, lanzar una excepción diferente o devolver un mensaje genérico
                throw new Exception("Ocurrió un error al buscar el programa de televisión.", ex);
            }
        }        
    }
}
