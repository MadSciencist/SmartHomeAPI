using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Matty.Framework.Abstractions
{
    public interface IGenericRepository<T> : IDisposable where T : IEntity
    {
        ITransaction BeginTransaction();
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetManyFiltered(Expression<Func<T, bool>> predicate);
        Task<T> GetFiltered(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
