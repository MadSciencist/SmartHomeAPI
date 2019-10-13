using SmartHome.Core.Entities.Enums;
using System.Security.Claims;

namespace SmartHome.Core.Security
{
    public interface IAuthorizationProvider<in T> where T : class, new()
    {
        bool Authorize(T entity, ClaimsPrincipal principal, OperationType operation);
    }
}