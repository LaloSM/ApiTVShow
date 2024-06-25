using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Persistence;
using Tvshow.Infrastructure.Persistence;

namespace Tvshow.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable? _repositories;

        private readonly TvshowDbContext _context;

        public UnitOfWork(TvshowDbContext context)
        {
            _context = context;
        }

        //Las transacciones que se realicen van a queda en memoria, hasta que se lance este "Complete"

        public async Task<int> Complete()
        {

            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error en transacion", e);
            }

        }

        //Elimina la instancia del unitoofwork
        public void Dispose()
        {
            _context.Dispose();
        }

        //Hace la instancia del objeto repository sobre una entidad determinada
        public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories is null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;//puede ser product, category

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(RepositoryBase<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IAsyncRepository<TEntity>)_repositories[type]!;


        }
    }
}
