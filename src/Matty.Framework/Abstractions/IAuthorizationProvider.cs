using Matty.Framework.Enums;
using System.Security.Claims;

namespace Matty.Framework.Abstractions
{
    /// <summary>
    /// Generic authorization provider.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IAuthorizationProvider<in TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// Authorize entity for specific operation.
        /// </summary>
        /// <param name="entity">Entity to authorize.</param>
        /// <param name="principal">Current user.</param>
        /// <param name="operation">Performing operation.</param>
        /// <returns>True when user is authorized, false otherwise.</returns>
        bool Authorize(TEntity entity, ClaimsPrincipal principal, OperationType operation);
    }
}