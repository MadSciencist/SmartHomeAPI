using Microsoft.AspNetCore.Identity;

namespace SmartHome.Domain.Role
{
    public class AppRole : IdentityRole<int>
    {
        public AppRole(string name) : base(name)
        {
        }
    }
}
