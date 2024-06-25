using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvshow.Application.Specifications
{
    public abstract class SpecificationParams
    {
        // Propiedad para especificar el criterio de ordenamiento
        public string? Sort { get; set; }

        // Índice de la página actual, inicializado a 1 por defecto
        public int PageIndex { get; set; } = 1;

        // Constante que define el tamaño máximo de la página permitido
        private const int MaxPageSize = 50;

        // Campo privado para el tamaño de página predeterminado
        private int _pagesize = 3;

        // Propiedad para el tamaño de página, limitado a un máximo de MaxPageSize
        public int PageSize
        {
            get => _pagesize;  // Devuelve el tamaño de página actual
            set => _pagesize = (value > MaxPageSize) ? MaxPageSize : value;  // Asigna el tamaño de página, asegurando que no sea mayor que MaxPageSize
        }

        // Propiedad para el término de búsqueda
        public string? Search { get; set; }
    }

}
