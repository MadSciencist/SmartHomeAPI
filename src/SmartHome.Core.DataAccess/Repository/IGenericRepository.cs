using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SmartHome.Core.Entities.Abstractions;
using SmartHome.Core.Entities.Entity;

namespace SmartHome.Core.DataAccess.Repository
{
    public interface IGenericRepository<T> where T : EntityBase, new()
    {
        IDatabaseTransaction BeginTransaction();
        // this needs to be cleaned asap to move interfaces to entity assembly
        IQueryable<T> AsQueryableNoTrack();

        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetManyFiltered(Expression<Func<T, bool>> predicate);
        Task<T> GetFiltered(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}