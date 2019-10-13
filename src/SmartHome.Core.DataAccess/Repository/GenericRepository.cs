﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Entities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Http;
using SmartHome.Core.Entities.Abstractions;
using SmartHome.Core.Entities.Utils;

namespace SmartHome.Core.DataAccess.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : EntityBase, new()
    {
        protected ILifetimeScope Container { get; }

        private EntityFrameworkContext _context;

        public EntityFrameworkContext Context => _context ?? (_context = Container.Resolve<EntityFrameworkContext>());

        private ILogger _logger;
        protected ILogger Logger => _logger ?? (_logger = Container.Resolve<ILoggerFactory>().CreateLogger(this.GetType().FullName));

        private IHttpContextAccessor _accessor;
        protected IHttpContextAccessor HttpContextAccessor =>_accessor ?? (_accessor = Container.Resolve<IHttpContextAccessor>());

        public GenericRepository(ILifetimeScope container)
        {
            Container = container;
        }

        public IDatabaseTransaction BeginTransaction()
            => new EntityFrameworkTransaction(Context);

        public virtual async Task<T> CreateAsync(T entity)
        {
            var currentUser = HttpContextAccessor.HttpContext.User;

            if (entity is ICreationAudit audit)
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

        public virtual async Task<T> GetFiltered(Expression<Func<T, bool>> predicate)
            => await Context.Set<T>().FirstOrDefaultAsync(predicate);

        public virtual async Task<IEnumerable<T>> GetManyFiltered(Expression<Func<T, bool>> predicate)
            => await Context.Set<T>().Where(predicate).ToListAsync();

        public virtual async Task<IEnumerable<T>> GetAllAsync() 
            => await Context.Set<T>().ToListAsync();

        public virtual async Task<T> GetByIdAsync(int id) 
            => await Context.Set<T>().FindAsync(id);

        public virtual async Task<T> UpdateAsync(T entity)
        {
            var currentUser = HttpContextAccessor.HttpContext.User;

            if (entity is IModificationAudit audit)
            {
                audit.Updated = DateTime.UtcNow;
                audit.UpdatedById = ClaimsPrincipalHelper.GetClaimedIdentifierInt(currentUser);
            }

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

        // TODO to refactor - dont want to return IQueryable

        public virtual IQueryable<T> AsQueryableNoTrack() => Context.Set<T>().AsNoTracking().AsQueryable();
    }
}