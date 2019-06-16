using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.Repository
{
    public interface IGenericRepository<T> where T : class, new()
    {
        AppDbContext Context { get; }
        IEnumerable<T> GetAll();
        IQueryable<T> AsQueryable();
        IQueryable<T> AsQueryableNoTrack();
        IEnumerable<T> Find(Func<T, bool> predicate);
        Task<T> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}