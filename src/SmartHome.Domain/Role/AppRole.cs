using Microsoft.AspNetCore.Identity;

namespace SmartHome.Core.Domain.Role
{
    public class AppRole : IdentityRole<int>
    {
        public AppRole(string name) : base(name)
        {
        }
    }
}
