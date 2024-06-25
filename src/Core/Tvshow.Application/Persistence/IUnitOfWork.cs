using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvshow.Application.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        //Trabaja sobre una entidad generica de los repositorios
        IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : class;

        Task<int> Complete();//Este metodo se dispara una vez que la operacion haya sido exitosa

    }
}
