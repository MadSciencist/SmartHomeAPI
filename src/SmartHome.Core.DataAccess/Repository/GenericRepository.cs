using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SmartHome.Core.DataAccess.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        public AppDbContext Context { get; }
        protected ILogger Logger { get; private set; }

        public GenericRepository(AppDbContext context, ILoggerFactory loggerFactory)
        {
            Context = context;
            Logger = loggerFactory.CreateLogger(nameof(GenericRepository<T>));
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            await Context.AddAsync(entity);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                Logger.LogError(e, "Error while creating entity - concurrency");
                throw;
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(e, "Error while creating entity");
                throw;
            }

            return entity;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            Context.Remove(entity);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                Logger.LogError(e, "Error while updating entity - concurrency");
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(e, "Error while updating entity");
                throw;
            }
        }

        public virtual IEnumerable<T> Find(Func<T, bool> predicate) => Context.Set<T>().Where(predicate);

        public virtual IEnumerable<T> GetAll() => Context.Set<T>();

        public virtual IQueryable<T> AsQueryable() => Context.Set<T>().AsQueryable();

        public virtual IQueryable<T> AsQueryableNoTrack() => Context.Set<T>().AsNoTracking().AsQueryable();

        public virtual async Task<T> GetByIdAsync(int id) => await Context.Set<T>().FindAsync(id);

        public virtual async Task<T> UpdateAsync(T entity)
        {
            Context.Set<T>().Update(entity);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                Logger.LogError(e, "Error while updating entity - concurrency");
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(e, "Error while updating entity");
                throw;
            }

            return entity;
        }
    }
}