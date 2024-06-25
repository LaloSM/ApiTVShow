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

namespace Tvshow.Application.Features.Tvshow.Commands.DeleteTvShow
{
    // Manejador de comando para eliminar un programa de televisión.
    public class DeleteTvShowCommandHandler : IRequestHandler<DeleteTvShowCommand, TvShowVm>
    {
        private readonly IUnitOfWork _unitOfWork; // Unidad de trabajo para interactuar con la capa de persistencia.
        private readonly IMapper _mapper; // Instancia del mapeador para convertir entidades a DTOs.

        // Constructor que inicializa la unidad de trabajo y el mapeador.
        public DeleteTvShowCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Método que maneja la solicitud de comando para eliminar un programa de televisión.
        public async Task<TvShowVm> Handle(DeleteTvShowCommand request, CancellationToken cancellationToken)
        {
            // Busca el programa de televisión que se desea actualizar por su ID.
            var tvshowToUpdate = await _unitOfWork.Repository<TvShow>().GetByIdAsync(request.IdTvShow);
            if (tvshowToUpdate == null)
            {
                throw new NotFoundException(nameof(TvShow), request.IdTvShow); // Lanza una excepción si el programa de televisión no fue encontrado.
            }

            tvshowToUpdate.Favorite = false; // Marca el programa de televisión como no favorito (ejemplo de actualización de propiedad).

            await _unitOfWork.Repository<TvShow>().UpdateAsync(tvshowToUpdate); // Actualiza el programa de televisión en la base de datos.

            return _mapper.Map<TvShowVm>(tvshowToUpdate); // Mapea y retorna el programa de televisión actualizado como un objeto TvShowVm.
        }
    }
}
