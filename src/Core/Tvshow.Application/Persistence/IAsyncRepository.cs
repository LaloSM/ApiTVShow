using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Specifications;

namespace Tvshow.Application.Persistence
{
    public interface IAsyncRepository<T> where T : class
    {
        // Define la firma del método para obtener todos los elementos de manera asincrónica.
        // Devuelve una tarea que eventualmente producirá una lista de solo lectura de objetos de tipo T.
        Task<IReadOnlyList<T>> GetAllAsync();

        // Define la firma del método para obtener elementos que cumplen con un predicado específico de manera asincrónica.
        // Toma como parámetro una expresión lambda que filtra los elementos del tipo T.
        // Devuelve una tarea que eventualmente producirá una lista de solo lectura de objetos de tipo T.
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);

        // Define la firma del método para obtener elementos de manera asincrónica, permitiendo especificar predicados,
        // ordenamiento, inclusión de propiedades relacionadas y seguimiento opcional.
        // - predicate: Una expresión lambda opcional para filtrar los elementos del tipo T.
        // - orderBy: Una función opcional que especifica cómo ordenar los resultados.
        // - includeString: Una cadena opcional que especifica propiedades relacionadas a incluir en la consulta.
        // - disableTracking: Un booleano opcional que indica si el seguimiento de entidades debe deshabilitarse (por defecto true).
        // Devuelve una tarea que eventualmente producirá una lista de solo lectura de objetos de tipo T.
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate,
                                       Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
                                       string? includeString,
                                       bool disableTracking = true);

        // Define la firma del método para obtener elementos de manera asincrónica, permitiendo especificar predicados,
        // ordenamiento, inclusión de propiedades relacionadas y seguimiento opcional.
        // - predicate: Una expresión lambda opcional para filtrar los elementos del tipo T.
        // - orderBy: Una función opcional que especifica cómo ordenar los resultados.
        // - includes: Una lista opcional de expresiones lambda que especifica propiedades relacionadas a incluir en la consulta.
        // - disableTracking: Un booleano opcional que indica si el seguimiento de entidades debe deshabilitarse (por defecto true).
        // Devuelve una tarea que eventualmente producirá una lista de solo lectura de objetos de tipo T.
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate,
                                       Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                       List<Expression<Func<T, object>>>? includes = null,
                                       bool disableTracking = true);

        // Define la firma del método para obtener una entidad específica de manera asincrónica, permitiendo especificar un predicado,
        // inclusión de propiedades relacionadas y seguimiento opcional.
        // - predicate: Una expresión lambda opcional para filtrar la entidad del tipo T.
        // - includes: Una lista opcional de expresiones lambda que especifica propiedades relacionadas a incluir en la consulta.
        // - disableTracking: Un booleano opcional que indica si el seguimiento de entidades debe deshabilitarse (por defecto true).
        // Devuelve una tarea que eventualmente producirá un objeto de tipo T.
        Task<T> GetEntityAsync(Expression<Func<T, bool>>? predicate,
                                         List<Expression<Func<T, object>>>? includes = null,
                                       bool disableTracking = true);

        // Define la firma del método para obtener una entidad específica de tipo T de manera asincrónica por su identificador.
        // - id: El identificador único de la entidad que se desea obtener.
        // Devuelve una tarea que eventualmente producirá un objeto de tipo T correspondiente al identificador proporcionado.
        Task<T> GetByIdAsync(int id);

        // Define la firma del método para agregar una entidad de tipo T de manera asincrónica.
        // - entity: La entidad de tipo T que se desea agregar.
        // Devuelve una tarea que eventualmente completará la operación de agregar la entidad y producirá la entidad agregada.
        Task<T> AddAsync(T entity);

        // Define la firma del método para actualizar una entidad de tipo T de manera asincrónica.
        // - entity: La entidad de tipo T que se desea actualizar.
        // Devuelve una tarea que eventualmente completará la operación de actualización de la entidad.
        Task<T> UpdateAsync(T entity);

        // Define la firma del método para eliminar una entidad de tipo T de manera asincrónica.
        // - entity: La entidad de tipo T que se desea eliminar.
        // Devuelve una tarea que eventualmente completará la operación de eliminación de la entidad.
        Task DeleteAsync(T entity);

        // Define el método para agregar una entidad de tipo T al repositorio.
        // - entity: La entidad de tipo T que se desea agregar.
        void AddEntity(T entity);

        void UpdateEntity(T entity);

        void DeleteEntity(T entity);

        //Agrega una lista de entidades al repositorio.
        void AddRange(List<T> entities);

        // Elimina un rango de entidades del repositorio.
        void DeleteRange(IReadOnlyList<T> entities);

        Task<T> GetByIdWithSpec(ISpecification<T> spec);

        Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
    }


}
