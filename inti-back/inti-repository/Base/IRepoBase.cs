using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.Base
{
    public interface IRepoBase<T> where T : class
    {
        int Count { get; }
        bool HasChanges { get; }
        T GetFirst();
        IEnumerable<T> GetAll();

        IEnumerable<T> GetRange(int skip, int take);
        int Add(T entity, bool persist = true);
        int AddRange(IEnumerable<T> entities, bool persist = true);
        int Update(T entity, bool persist = true);
        int UpdateRange(IEnumerable<T> entities, bool persist = true);
        int Delete(T entity, bool persist = true);
        int DeleteRange(IEnumerable<T> entities, bool persist = true);

        int SaveChanges();
    }
}
