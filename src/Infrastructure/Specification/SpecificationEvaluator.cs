using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Specifications;

namespace Tvshow.Infrastructure.Specification
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            // Aplica el criterio de filtro especificado por la especificación (si existe).
            if (spec.Criteria != null)
            {
                inputQuery = inputQuery.Where(spec.Criteria);
            }

            // Aplica el orden ascendente especificado por la especificación (si existe).
            if (spec.OrderBy != null)
            {
                inputQuery = inputQuery.OrderBy(spec.OrderBy);
            }

            // Aplica el orden descendente especificado por la especificación (si existe).
            if (spec.OrderByDescending != null)
            {
                inputQuery = inputQuery.OrderByDescending(spec.OrderByDescending);
            }

            // Habilita la paginación si está configurada en la especificación.
            if (spec.IsPaginEnable)
            {
                inputQuery = inputQuery.Skip(spec.Skip).Take(spec.Take);
                // Salta los primeros 'spec.Skip' registros y toma los siguientes 'spec.Take' registros.
            }

            // Incluye las entidades relacionadas especificadas por la especificación.
            inputQuery = spec.Includes!.Aggregate(inputQuery,
                (current, include) => current.Include(include))
                .AsSplitQuery() // Optimiza la consulta para separar las consultas de carga de datos relacionados.
                .AsNoTracking(); // Indica que los resultados no deben rastrearse por cambios.

            // Retorna la consulta IQueryable resultante después de aplicar todas las especificaciones.
            return inputQuery;
        }
    }
}
