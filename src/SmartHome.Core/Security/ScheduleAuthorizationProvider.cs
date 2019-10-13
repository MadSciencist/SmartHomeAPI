using Matty.Framework.Abstractions;
using Matty.Framework.Enums;
using Matty.Framework.Utils;
using SmartHome.Core.Entities.SchedulingEntity;
using System.Security.Claims;

namespace SmartHome.Core.Security
{
    public class ScheduleAuthorizationProvider : AuthorizationProviderBase, IAuthorizationProvider<ScheduleEntity>
    {
        public bool Authorize(ScheduleEntity entity, ClaimsPrincipal principal, OperationType operation)
        {
            if (principal is null) return false;
            if (IsAdmin(principal) || IsSystemUser(principal)) return true;

            return entity.CreatedById == int.Parse(ClaimsPrincipalHelper.GetClaimedIdentifier(principal));
        }
    }
}
