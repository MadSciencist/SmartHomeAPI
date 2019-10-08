using Microsoft.AspNetCore.Identity;

namespace SmartHome.Core.Entities.Role
{
    public class AppRole : IdentityRole<int>
    {
        public AppRole(string name) : base(name)
        {
        }
    }
}
