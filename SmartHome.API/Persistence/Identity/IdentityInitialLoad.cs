using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartHome.Domain.Role;
using SmartHome.Domain.User;

namespace SmartHome.API.Persistence.Identity
{
    public static class IdentityInitialLoad
    {
        public static async Task Seed(IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
                var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
                    .CreateLogger(nameof(IdentityInitialLoad));

                if (!await roleManager.RoleExistsAsync("admin")
                    && !await roleManager.RoleExistsAsync("user")
                    && !await roleManager.RoleExistsAsync("sensor"))
                {
                    logger.LogInformation("Creating roles");
                    await roleManager.CreateAsync(new AppRole("admin"));
                    await roleManager.CreateAsync(new AppRole("user"));
                    await roleManager.CreateAsync(new AppRole("sensor"));
                }

                if (await userManager.FindByNameAsync("admin") != null) return;

                var adminUser = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    EmailConfirmed = true,
                    IsActive = true,
                    ActivatedBy = null,
                    ActivationDate = DateTime.UtcNow
                };

                const string password = "admin1";

                logger.LogInformation("Creating admin user");
                await userManager.CreateAsync(adminUser, password);
                await userManager.AddToRoleAsync(adminUser, "admin");
                await userManager.AddToRoleAsync(adminUser, "user");
                await userManager.AddToRoleAsync(adminUser, "sensor");
            }
        }
    }
}
