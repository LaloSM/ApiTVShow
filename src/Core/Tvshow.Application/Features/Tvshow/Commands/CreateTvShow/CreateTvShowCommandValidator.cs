using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvshow.Application.Features.Tvshow.Commands.CreateTvShow
{
    public class CreateTvShowCommandValidator : AbstractValidator<CreateTvShowCommand>
    {
        public CreateTvShowCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("El campo nombre no permite valores nulos")
                .NotNull().WithMessage("El campo Nombre no permite valores nulos.");

            RuleFor(p => p.Favorite)
               .NotEmpty().WithMessage("El campo nombre no permite valores nulos")
               .NotNull().WithMessage("El campo favorito no permite valores nulos");
        }
    }
}
