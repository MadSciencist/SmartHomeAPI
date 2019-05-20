using System.Security.Claims;

namespace SmartHome.Core.Services
{
    public abstract class ServiceBase
    {
        public virtual ClaimsPrincipal ClaimsOwner { get; set; }
    }
}