using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inti_model;

namespace inti_repository.Base
{
    public abstract class RepoBase<T> : IDisposable, IRepoBase<T> where T : class, new()
    {
        protected readonly IntiDBContext intiDBContext;
        protected DbSet<T> Table;

        public IntiDBContext Context => intiDBContext;

        protected RepoBase()
        {
            intiDBContext = new IntiDBContext();
            Table = intiDBContext.Set<T>();
        }

        protected RepoBase(DbContextOptions<IntiDBContext> options)
        {
            intiDBContext = new IntiDBContext(options);
        }

        bool _dispose = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (_dispose) return;

            if (dispose)
            {
                intiDBContext.Dispose();
                _dispose = true;
            }
        }

        public int Count => Table.Count();

        public bool HasChanges => intiDBContext.ChangeTracker.HasChanges();

        public int Add(T entity, bool persist = true)
        {
            Table.Add(entity);

            return persist ? SaveChanges() : 0;
        }

        public int AddRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.AddRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public int Delete(T entity, bool persist = true)
        {
            Table.Remove(entity);
            return persist ? SaveChanges() : 0;
        }

        public int DeleteRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.RemoveRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual IEnumerable<T> GetAll() => Table;


        public T GetFirst() => Table.FirstOrDefault();

        public IEnumerable<T> GetRange(IQueryable<T> query, int skip, int take) => query.Skip(skip).Take(take);
        public virtual IEnumerable<T> GetRange(int skip, int take) => GetRange(Table, skip, take);

        public int SaveChanges()
        {
            try
            {
                return intiDBContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {

                Console.WriteLine(ex);
                throw;
            }
            catch (RetryLimitExceededException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public virtual int Update(T entity, bool persist = true)
        {
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public int UpdateRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.UpdateRange(entities);
            return persist ? SaveChanges() : 0;
        }
    }
}
