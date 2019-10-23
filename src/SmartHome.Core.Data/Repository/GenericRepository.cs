using Autofac;
using Matty.Framework.Abstractions;
using Matty.Framework.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmartHome.Core.Data.Repository
{
    public class GenericRepository<TEntity, TKey> : ITransactionalRepository<TEntity, TKey> where TEntity : class, IEntity, new()
    {
        protected ILifetimeScope Container { get; }

        private EntityFrameworkContext _context;

        public EntityFrameworkContext Context => _context ??= Container.Resolve<EntityFrameworkContext>();

        private ILogger _logger;
        protected ILogger Logger => _logger ??= Container.Resolve<ILoggerFactory>().CreateLogger(this.GetType().FullName);

        private IHttpContextAccessor _accessor;
        protected IHttpContextAccessor HttpContextAccessor => _accessor ??= Container.Resolve<IHttpContextAccessor>();

        public GenericRepository(ILifetimeScope container)
        {
            Container = container;
        }

        public ITransaction BeginTransaction()
            => new EntityFrameworkTransaction();

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            var currentUser = HttpContextAccessor.HttpContext?.User;

            if (entity is ICreationAudit<AppUser, int> audit)
            {
                audit.Created = DateTime.UtcNow;
                audit.CreatedById = ClaimsPrincipalHelper.GetClaimedIdentifierInt(currentUser);
            }

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

        public virtual async Task DeleteAsync(TEntity entity)
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

        public virtual async Task<TEntity> GetFiltered(Expression<Func<TEntity, bool>> predicate)
            => await Context.Set<TEntity>().FirstOrDefaultAsync(predicate);

        public virtual async Task<IEnumerable<TEntity>> GetManyFiltered(Expression<Func<TEntity, bool>> predicate)
            => await Context.Set<TEntity>().Where(predicate).ToListAsync();

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
            => await Context.Set<TEntity>().ToListAsync();

        public virtual async Task<TEntity> GetByIdAsync(TKey id)
            => await Context.Set<TEntity>().FindAsync(id);

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var currentUser = HttpContextAccessor.HttpContext?.User;

            if (entity is IModificationAudit<AppUser, int?> audit)
            {
                audit.Updated = DateTime.UtcNow;
                audit.UpdatedById = ClaimsPrincipalHelper.GetClaimedIdentifierInt(currentUser);
            }

            Context.Entry(entity).State = EntityState.Modified;

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
    }
}