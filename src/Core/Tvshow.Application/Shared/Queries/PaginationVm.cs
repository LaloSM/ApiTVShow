using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvshow.Application.Shared.Queries
{
    public class PaginationVm<T> where T : class
    {
        public int Count { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public IReadOnlyList<T>? Data { get; set; }//devuleve la conexion de objetos generico
        public int PageCount { get; set; }
        public int ResultByPage { get; set; }
    }
}
