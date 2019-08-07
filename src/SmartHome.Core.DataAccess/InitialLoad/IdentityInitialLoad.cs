using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Role;
using SmartHome.Core.Domain.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.InitialLoad
{
    public class IdentityInitialLoad
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger _logger;

        public IdentityInitialLoad(IServiceProvider provider)
        {
            _provider = provider;
            var loggerFactory = _provider.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
            _logger = loggerFactory?.CreateLogger(nameof(IdentityInitialLoad));
        }

        public async Task SeedRoles()
        {
            using (var scope = _provider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
   
                if (!await roleManager.RoleExistsAsync("admin")
                    && !await roleManager.RoleExistsAsync("user")
                    && !await roleManager.RoleExistsAsync("sensor"))
                {
                    _logger.LogInformation("Creating roles");
                    await roleManager.CreateAsync(new AppRole("admin"));
                    await roleManager.CreateAsync(new AppRole("user"));
                    await roleManager.CreateAsync(new AppRole("sensor"));
                }
            }
        }

        public async Task SeedUsers()
        {
            using (var scope = _provider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                if (await userManager.FindByNameAsync("admin") != null) return;

                // id = 1
                var adminUser = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    EmailConfirmed = true,
                    IsActive = true,
                    ActivatedBy = null,
                    ActivationDate = DateTime.UtcNow,
                    UiConfiguration = new List<UiConfiguration>
                    {
                        new UiConfiguration
                        {
                            Type = Domain.Enums.UiConfigurationType.Dashboard,
                            Data = "{}",
                            UserId = 1,
                        },
                        new UiConfiguration
                        {
                            Type = Domain.Enums.UiConfigurationType.Control,
                            Data = "{}",
                            UserId = 1,
                        }
                    }
                };

                const string password = "admin1";

                _logger.LogInformation("Creating admin user");
                await userManager.CreateAsync(adminUser, password);
                await userManager.AddToRoleAsync(adminUser, "admin");
                await userManager.AddToRoleAsync(adminUser, "user");
                await userManager.AddToRoleAsync(adminUser, "sensor");
            }
        }
    }
}