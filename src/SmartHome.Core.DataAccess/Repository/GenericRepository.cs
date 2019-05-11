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
        protected readonly AppDbContext _context;
        private readonly ILogger _logger;

        public GenericRepository(AppDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("Repository logger");
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            await _context.AddAsync(entity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(e, "Error while updading entity - concurrency");
                throw;
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error while updading entity");
                throw;
            }

            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Remove(entity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(e, "Error while updating entity - concurrency");
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error while updating entity");
                throw;
            }
        }

        public IEnumerable<T> Find(Func<T, bool> predicate) => _context.Set<T>().Where(predicate);

        public IEnumerable<T> GetAll() => _context.Set<T>();

        public IQueryable<T> AsQueryable() => _context.Set<T>().AsQueryable();

        public IQueryable<T> AsQueryableNoTrack() => _context.Set<T>().AsNoTracking().AsQueryable();

        public virtual async Task<T> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(e, "Error while updating entity - concurrency");
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error while updating entity");
                throw;
            }

            return entity;
        }
    }
}