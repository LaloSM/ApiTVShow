using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Exceptions;
using Tvshow.Application.Features.Tvshow.Vms;
using Tvshow.Application.Persistence;
using Tvshow.Domain;

namespace Tvshow.Application.Features.Tvshow.Commands.UpdateTvShow
{
    public class UpdateTvShowCommandHandler : IRequestHandler<UpdateTvShowCommand, TvShowVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateTvShowCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TvShowVm> Handle(UpdateTvShowCommand request, CancellationToken cancellationToken)
        {
            var tvshowToUpdate = await _unitOfWork.Repository<TvShow>().GetByIdAsync(request.IdTvShow);

            if (tvshowToUpdate == null)
            {
                throw new NotFoundException(nameof(TvShow), request.IdTvShow);
            }

            try
            {
                // Mapear los datos del comando de actualización a la entidad existente
                _mapper.Map(request, tvshowToUpdate);

                // Actualizar los canales en la base de datos
                await _unitOfWork.Repository<TvShow>().UpdateAsync(tvshowToUpdate);

                // Mapear la entidad Tvshow actualizada a DTO y devolverla
                var tvshowDto = _mapper.Map<TvShowVm>(tvshowToUpdate);
                return tvshowDto;
            }
            catch (Exception ex)
            {
                // Capturar y registrar cualquier excepción inesperada
                // Aquí podrías registrar la excepción en tu sistema de log
                throw new Exception("Ocurrió un error al actualizar los programas.", ex);
            }
        }
    }
}
