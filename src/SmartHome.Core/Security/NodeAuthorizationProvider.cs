﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Utils;

namespace SmartHome.Core.Security
{
    public class NodeAuthorizationProvider
    {
        public bool Authorize(Node node, ClaimsPrincipal principal, OperationType operation)
        {
            if (principal is null) return false;
            if (IsAdmin(principal)) return true;

            switch (operation)
            {
                case OperationType.Add:
                    return HandleAddPermission(principal);

                case OperationType.Execute:
                case OperationType.Read:
                    return HandleReadPermission(node, principal);

                case OperationType.Modify:
                    return HandleModifyPermission(node, principal);

                case OperationType.Delete:
                case OperationType.HardDelete:
                    return HandleDeletePermission(node, principal);

                default:
                    return false;
            }
        }

        private static bool IsAdmin(IPrincipal principal)
        {
            return principal.IsInRole(Roles.Admin);
        }

        private static bool HandleAddPermission(IPrincipal principal)
        {
            return principal.IsInRole(Roles.User) || principal.IsInRole(Roles.Admin);
        }

        public static bool HandleReadPermission(Node node, ClaimsPrincipal principal)
        {
            return principal.IsInRole(Roles.User) && (IsOwnerOfNode(node, principal) || IsEligible(node, principal));
        }

        public static bool HandleModifyPermission(Node node, ClaimsPrincipal principal)
        {
            return principal.IsInRole(Roles.User) && (IsOwnerOfNode(node, principal) || IsEligible(node, principal));
        }

        public static bool HandleDeletePermission(Node node, ClaimsPrincipal principal)
        {
            return principal.IsInRole(Roles.User) && (IsOwnerOfNode(node, principal) || IsEligible(node, principal));
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