using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tvshow.Application.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        // Constructor sin parámetros
        public BaseSpecification()
        {

        }

        // Constructor que recibe un criterio de filtro
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        // Propiedad que almacena el criterio de filtro como una expresión lambda
        public Expression<Func<T, bool>>? Criteria { get; }

        // Lista de expresiones lambda para incluir entidades relacionadas
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, Object>>>();

        // Expresión lambda para el orden ascendente
        public Expression<Func<T, object>>? OrderBy { get; private set; }

        // Expresión lambda para el orden descendente
        public Expression<Func<T, object>>? OrderByDescending { get; private set; }

        // Número de elementos a tomar (para la paginación)
        public int Take { get; private set; }

        // Número de elementos a omitir (para la paginación)
        public int Skip { get; private set; }

        // Indica si la paginación está habilitada
        public bool IsPaginEnable { get; private set; }

        // Método protegido para establecer la expresión lambda de orden ascendente
        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        // Método protegido para establecer la expresión lambda de orden descendente
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByExpression)
        {
            OrderByDescending = orderByExpression;
        }

        // Método protegido para aplicar paginación
        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPaginEnable = true;
        }

        // Método protegido para agregar expresiones lambda de entidades relacionadas
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
    }


}
