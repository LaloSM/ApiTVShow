using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tvshow.Application.Specifications
{
    public interface ISpecification<T> // Interfaz Generica
    {
        Expression<Func<T, bool>>? Criteria { get; }   //Busqueda de filtros, expresion de busqueda
        List<Expression<Func<T, object>>>? Includes { get; }//Devuelve una lista de entidades vinvuladas
        Expression<Func<T, object>>? OrderBy { get; } //Ordenamiento ascendente
        Expression<Func<T, object>>? OrderByDescending { get; } //Ordenamiento descendente
        int Take { get; } //Toma un conjunto de records
        int Skip { get; } //Indica hasta que elemento lo va a vincular
        bool IsPaginEnable { get; }//Habilita la paginacion
    }
}
