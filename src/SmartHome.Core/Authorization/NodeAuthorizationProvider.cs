using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Utils;
using System;
using System.Linq;
using System.Security.Claims;

namespace SmartHome.Core.Authorization
{
    public class NodeAuthorizationProvider
    {
        public bool Authorize(Node node, ClaimsPrincipal principal)
        {
            return IsAdmin(principal) || IsOwnerOfNode(node, principal) || IsEligible(node, principal);
        }

        private static bool IsAdmin(ClaimsPrincipal principal)
        {
            return ClaimsPrincipalHelper.IsUserAdmin(principal);
        }

        private static bool IsOwnerOfNode(Node node, ClaimsPrincipal principal)
        {
            return node.CreatedById == Convert.ToInt32(ClaimsPrincipalHelper.GetClaimedIdentifier(principal));
        }

        private static bool IsEligible(Node node, ClaimsPrincipal principal)
        {
            return node.AllowedUsers.Any(x => x.UserId == Convert.ToInt32(ClaimsPrincipalHelper.GetClaimedIdentifier(principal)));
        }
    }
}
