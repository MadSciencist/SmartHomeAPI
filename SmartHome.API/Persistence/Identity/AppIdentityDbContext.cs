using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartHome.Domain.Role;
using SmartHome.Domain.User;

namespace SmartHome.API.Persistence.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>().ToTable("appuser");
        }
    }
}