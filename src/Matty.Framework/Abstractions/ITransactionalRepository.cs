namespace Matty.Framework.Abstractions
{
    /// <summary>
    /// Extends IGenericRepository with option of transaction
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TKey">Type of entity primary key</typeparam>
    public interface ITransactionalRepository<TEntity, in TKey> : IGenericRepository<TEntity, TKey> where TEntity : IEntity
    {
        /// <summary>
        /// Starting new transaction
        /// </summary>
        /// <returns>ITransaction object</returns>
        ITransaction BeginTransaction();
    }
}
