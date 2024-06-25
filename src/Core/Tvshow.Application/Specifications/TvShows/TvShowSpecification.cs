using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Domain;

namespace Tvshow.Application.Specifications.TvShows
{
    public class TvShowSpecification : BaseSpecification<TvShow>
    {
        public TvShowSpecification(TvShowSpecificationParams showsParams)
        // Constructor de la clase TvShowSpecification que recibe TvShowSpecificationParams
        : base(
            x =>
                string.IsNullOrEmpty(showsParams.Name)  // Criterio de filtro: Evalúa si el nombre en showsParams es nulo o vacío
          )
            {
            // Cuerpo del constructor

            // Aplica paginación utilizando los parámetros PageSize y PageIndex de showsParams
            ApplyPaging(showsParams.PageSize * (showsParams.PageIndex - 1), showsParams.PageSize);

            // Verifica si se especificó un criterio de ordenamiento en showsParams
            if (!string.IsNullOrEmpty(showsParams.Sort))
            {
                // Switch para manejar diferentes opciones de ordenamiento
                switch (showsParams.Sort)
                {
                    case "tvshowAsc":
                        // Agrega un orden ascendente por el nombre del programa de televisión
                        AddOrderBy(p => p.Name!);
                        break;

                    case "tvshowDesc":
                        // Agrega un orden descendente por el nombre del programa de televisión
                        AddOrderByDescending(p => p.Name!);
                        break;

                    default:
                        // Orden predeterminado por fecha de creación si no se especifica un criterio válido
                        AddOrderBy(p => p.CreatedDate!);
                        break;
                }
            }
            else
            {
                // Orden predeterminado por nombre en orden descendente si no se especifica ningún criterio de ordenamiento
                AddOrderByDescending(p => p.Name!);
            }
        }

    }
}
