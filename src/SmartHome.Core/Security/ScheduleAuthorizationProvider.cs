using SmartHome.Core.Entities.Enums;
using SmartHome.Core.Entities.SchedulingEntity;
using SmartHome.Core.Entities.Utils;
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
