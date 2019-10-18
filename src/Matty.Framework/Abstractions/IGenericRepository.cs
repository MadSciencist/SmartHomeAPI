using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Matty.Framework.Abstractions
{
    public interface IGenericRepository<TEntity, TKey> : IDisposable where TEntity : IEntity
    {
        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>Collection of TEntity type</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Gets entities filtered by predicate
        /// </summary>
        /// <param name="predicate">The predicate to evaluate.</param>
        /// <returns>Collection of TEntity matching predicate criteria</returns>
        Task<IEnumerable<TEntity>> GetManyFiltered(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets single entity matching predicate
        /// </summary>
        /// <param name="predicate">The predicate to evaluate.</param>
        /// <returns>Single TEntity matching predicate criteria</returns>
        Task<TEntity> GetFiltered(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets entity by it's primary key
        /// </summary>
        /// <param name="id">Key of entity</param>
        /// <returns>Single entity with given key</returns>
        Task<TEntity> GetByIdAsync(TKey id);

        /// <summary>
        /// Creates new entity
        /// </summary>
        /// <param name="entity">Entity to create</param>
        /// <returns>Created entity</returns>
        Task<TEntity> CreateAsync(TEntity entity);

        /// <summary>
        /// Updates existing entity
        /// </summary>
        /// <param name="entity">Existing entity</param>
        /// <returns>Updated entity</returns>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Deletes entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        /// <returns></returns>
        Task DeleteAsync(TEntity entity);
    }
}
