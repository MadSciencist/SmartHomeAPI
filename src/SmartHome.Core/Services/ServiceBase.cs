using SmartHome.Core.Utils;
using System;
using System.Security.Claims;

namespace SmartHome.Core.Services
{
    public abstract class ServiceBase : IServiceBase
    {
        public virtual ClaimsPrincipal Principal { get; set; }

        public virtual int GetCurrentUserId(ClaimsPrincipal principal)
        {
            return Convert.ToInt32(ClaimsPrincipalHelper.GetClaimedIdentifier(principal));
        }
    }
}