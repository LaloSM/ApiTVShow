using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Domain;

namespace Tvshow.Application.Specifications.TvShows
{
    public class TvShowForCountingSpecification : BaseSpecification<TvShow>
    {
        public TvShowForCountingSpecification(TvShowSpecificationParams tvshowParams)
        // Constructor de la clase TvShowForCountingSpecification que recibe TvShowSpecificationParams
        : base(
            x =>
                (
                    string.IsNullOrEmpty(tvshowParams.Search)  // Evalúa si el parámetro de búsqueda en tvshowParams es nulo o vacío
                )
          )
            {
                // Cuerpo del constructor
                // Aquí normalmente se realizarían más configuraciones específicas de la clase, pero en este caso no hay más código visible
            }
        }
}
