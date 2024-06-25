using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvshow.Application.Shared.Queries
{
    public class PaginationBaseQuery
    {
        // Propiedad para el término de búsqueda
        public string? Search { get; set; }

        // Propiedad para el criterio de ordenamiento
        public string? Sort { get; set; }

        // Propiedad para el índice de la página actual, inicializado a 1 por defecto
        public int PageIndex { get; set; } = 1;

        // Campo privado para el tamaño de página predeterminado
        private int _pageSize = 3;

        // Constante que define el tamaño máximo de página permitido
        private const int MaxPageSize = 50;

        // Propiedad para el tamaño de página, con validación para limitar a MaxPageSize
        public int PageSize
        {
            get => _pageSize;  // Devuelve el tamaño de página actual
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;  // Asigna el tamaño de página, asegurando que no sea mayor que MaxPageSize
        }
    }

}
