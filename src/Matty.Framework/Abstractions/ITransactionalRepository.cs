namespace Matty.Framework.Abstractions
{
    /// <summary>
    /// Extends IGenericRepository with option of transaction
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface ITransactionalRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : IEntity
    {
        /// <summary>
        /// Starting new transaction
        /// </summary>
        /// <returns>ITransaction object</returns>
        ITransaction BeginTransaction();
    }
}
