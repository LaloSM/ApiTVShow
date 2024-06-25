using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Features.Tvshow.Vms;
using Tvshow.Application.Persistence;
using Tvshow.Domain;

namespace Tvshow.Application.Features.Tvshow.Commands.CreateTvShow
{
    // Manejador de comando para crear un nuevo programa de televisión.
    public class CreateTvShowCommandHandler : IRequestHandler<CreateTvShowCommand, TvShowVm>
    {
        private readonly IMapper _mapper; // Instancia del mapeador para convertir entidades a DTOs.
        private readonly IUnitOfWork _unitOfWork; // Unidad de trabajo para interactuar con la capa de persistencia.

        // Constructor que inicializa el mapeador y la unidad de trabajo.
        public CreateTvShowCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // Método que maneja la solicitud de comando para crear un nuevo programa de televisión.
        public async Task<TvShowVm> Handle(CreateTvShowCommand request, CancellationToken cancellationToken)
        {
            // Validar el comando usando FluentValidation
            var validator = new CreateTvShowCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                // Si la validación no es válida, se obtienen los mensajes de error personalizados y se lanza una excepción de validación.
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ValidationException(string.Join(", ", errors));
            }

            // Determinar si el programa de televisión es favorito basado en el valor de request.Favorite
            bool isFavorite = (request.Favorite == 1);

            // Crear una nueva entidad de programa de televisión con los datos proporcionados en el comando.
            var tvshowEntity = new TvShow
            {
                Name = request.Name,
                Favorite = isFavorite
            };

            // Agregar la entidad de programa de televisión a la unidad de trabajo para persistirla en la base de datos.
            _unitOfWork.Repository<TvShow>().AddEntity(tvshowEntity);

            // Completar la operación de guardado en la base de datos.
            var result = await _unitOfWork.Complete();
            if (result <= 0)
            {
                throw new Exception("No se pudo guardar el programa de televisión");
            }

            // Retornar el programa de televisión recién creado convertido a TvShowVm mediante el mapeador.
            return _mapper.Map<TvShowVm>(tvshowEntity);
        }
    }

}
