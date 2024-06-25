using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Persistence;
using Tvshow.Application.Specifications;
using Tvshow.Infrastructure.Persistence;
using Tvshow.Infrastructure.Specification;

namespace Tvshow.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IAsyncRepository<T> where T : class
    {
        protected readonly TvshowDbContext _context;//Esta es la instancia que se hace para hacer las operaciones en base de datos

        public RepositoryBase(TvshowDbContext context)
        {
            _context = context;
        }

        // Inserta una entidad en la base de datos y guarda los cambios de manera asíncrona.
        public async Task<T> AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);  // Agrega la entidad al contexto de base de datos en memoria.
            await _context.SaveChangesAsync();  // Guarda los cambios en la base de datos real.
            return entity;  // Retorna la entidad agregada.
        }

        // Registra una entidad en la memoria de la aplicación.
        public void AddEntity(T entity)
        {
            _context.Set<T>().Add(entity);  // Agrega la entidad al contexto de base de datos en memoria.
        }

        // Agrega una lista de entidades en la memoria de la aplicación.
        public void AddRange(List<T> entities)
        {
            _context.Set<T>().AddRange(entities);  // Agrega una colección de entidades al contexto en memoria.
        }

        // Elimina una entidad de la base de datos de manera asíncrona y confirma los cambios.
        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);  // Marca la entidad para eliminación en el contexto de base de datos.
            await _context.SaveChangesAsync();  // Guarda los cambios en la base de datos real.
        }

        // Elimina una entidad de la memoria de la aplicación.
        public void DeleteEntity(T entity)
        {
            _context.Set<T>().Remove(entity);  // Marca la entidad para eliminación en el contexto de base de datos en memoria.
        }

        // Elimina una lista de entidades en la memoria de la aplicación y eliminacion en cascada
        public void DeleteRange(IReadOnlyList<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);  // Marca una colección de entidades para eliminación en el contexto de base de datos en memoria.
        }

        // Obtiene todas las entidades de tipo T de la base de datos de manera asíncrona.
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();  // Retorna todas las entidades de tipo T desde la base de datos.
        }

        //Referencia la entidad y le tenemos que pasar una expresion labmla
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, string? includeString, bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();
            if (disableTracking) query = query.AsNoTracking();

            //incluye las entidades 
            if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

            //lleva un predicado 
            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).ToListAsync();


            return await query.ToListAsync();
        }

        // Aca cambia porque aca ya te incluye una lista
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, List<Expression<Func<T, object>>>? includes = null, bool disableTracking = true)
        {

            IQueryable<T> query = _context.Set<T>(); // Crea una consulta IQueryable para la entidad T en el contexto de base de datos.
            if (disableTracking) query = query.AsNoTracking(); // Opcionalmente deshabilita el seguimiento de cambios para mejorar el rendimiento.

            // Ejemplo, si es un producto aca ya te va a incluir la imagenes y los reviews
            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include)); // Incluye relaciones relacionadas según sea necesario.

            if (predicate != null) query = query.Where(predicate); // Aplica un filtro basado en el predicado proporcionado.

            if (orderBy != null)
                return await orderBy(query).ToListAsync(); // Si hay una función de ordenación proporcionada, ordena el resultado y retorna la lista.

            return await query.ToListAsync(); // Ejecuta la consulta y retorna todas las entidades resultantes como una lista.
        }

        // Obtiene una entidad por su identificador único de manera asincrónica
        public async Task<T> GetByIdAsync(int id)
        {
            // Utiliza el contexto de base de datos para acceder al conjunto de entidades de tipo T
            return (await _context.Set<T>().FindAsync(id))!;

        }

        // Retorna una única entidad basada en un predicado, incluyendo entidades relacionadas opcionales, de manera asincrónica
        public async Task<T> GetEntityAsync(Expression<Func<T, bool>>? predicate, List<Expression<Func<T, object>>>? includes = null, bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();

            // Habilita o deshabilita el seguimiento de entidades según el parámetro disableTracking
            if (disableTracking)
                query = query.AsNoTracking();

            // Incluye entidades relacionadas especificadas en includes
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            // Aplica el predicado de filtrado si se proporciona
            if (predicate != null)
                query = query.Where(predicate);

            // Retorna la primera entidad que cumpla con el criterio o nula si no se encuentra ninguna
            return (await query.FirstOrDefaultAsync())!;
        }

        // Envia la transacción a la base de datos para actualizar una entidad existente de manera asincrónica
        public async Task<T> UpdateAsync(T entity)
        {
            // Adjunta la entidad al contexto de base de datos para comenzar a rastrearla
            _context.Set<T>().Attach(entity);

            // Marca la entidad como modificada en el contexto para que se actualice en la base de datos
            _context.Entry(entity).State = EntityState.Modified;

            // Guarda los cambios en la base de datos y espera la finalización de la operación
            await _context.SaveChangesAsync();

            // Retorna la entidad actualizada después de la operación de actualización
            return entity;
        }

        // Aplica cambios en la entidad específica dentro del contexto de base de datos en memoria
        public void UpdateEntity(T entity)
        {
            // Adjunta la entidad al contexto de base de datos para comenzar a rastrearla
            _context.Set<T>().Attach(entity);

            // Marca la entidad como modificada en el contexto de Entity Framework
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            // Aplica la especificación para obtener la consulta IQueryable correspondiente.
            IQueryable<T> query = ApplySpecification(spec);

            // Utiliza CountAsync para contar el número de entidades que cumplen con los criterios de la especificación.
            return await query.CountAsync();
        }

        // Retorna la primera entidad que cumple con los criterios especificados por la especificación.
        // Utiliza la especificación para aplicar filtros y otras operaciones antes de obtener la entidad.
        public async Task<T> GetByIdWithSpec(ISpecification<T> spec)
        {
            // Aplica la especificación para obtener la consulta IQueryable correspondiente y luego llama a FirstOrDefaultAsync para obtener la primera entidad que cumple con los criterios.
            return (await ApplySpecification(spec).FirstOrDefaultAsync())!;
        }

        // Retorna una lista de todas las entidades que cumplen con los criterios especificados por la especificación.
        // Utiliza la especificación para aplicar filtros y otras operaciones antes de obtener la lista de entidades.
        public async Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec)
        {
            // Aplica la especificación para obtener la consulta IQueryable correspondiente y luego llama a ToListAsync para obtener la lista de todas las entidades que cumplen con los criterios.
            return await ApplySpecification(spec).ToListAsync();
        }

        // Retorna una consulta IQueryable que ha sido filtrada y ordenada según la especificación proporcionada.
        public IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            // Llama a un método estático de la clase SpecificationEvaluator<T> para obtener la consulta IQueryable
            // Este método aplica los criterios de la especificación a la consulta IQueryable inicial.
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }
    }
}
