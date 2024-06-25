using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvshow.Application.Features.Tvshow.Commands.UpdateTvShow
{
    public class UpdateTvShowCommandValidator : AbstractValidator<UpdateTvShowCommand>
    {
        public UpdateTvShowCommandValidator()
        {
            RuleFor(p => p.Name)
               .NotNull().WithMessage("El campo nombre no permite valores nulos");

            RuleFor(p => p.Favorite)
               .NotNull().WithMessage("El campo favorito no permite valores nulos");
        }
    }
}
