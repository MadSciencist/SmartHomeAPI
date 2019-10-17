using Matty.Framework.Enums;
using System.Security.Claims;

namespace Matty.Framework.Abstractions
{
    public interface IAuthorizationProvider<in T> where T : class, new()
    {
        bool Authorize(T entity, ClaimsPrincipal principal, OperationType operation);
    }
}