using SmartHome.Core.Domain.Entity;
using System.Linq;

namespace SmartHome.Core.Authorization
{
    public class NodeAuthorizationProvider
    {
        public bool Authorize(Node node, int userId)
        {
           if(IsOwnerOfNode(node, userId) || IsEligible(node, userId))
            {
                return true;
            }

            return false;
        }

        private bool IsOwnerOfNode(Node node, int userId)
        {
            return node.CreatedById == userId;
        }

        private bool IsEligible(Node node, int userId)
        {
            return node.AllowedUsers.Any(x => x.UserId == userId);
        }
    }
}
