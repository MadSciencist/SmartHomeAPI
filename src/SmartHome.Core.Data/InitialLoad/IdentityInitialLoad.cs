﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Entities.Role;
using SmartHome.Core.Entities.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Data.InitialLoad
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
                    && !await roleManager.RoleExistsAsync("user"))
                {
                    _logger.LogInformation("Creating roles");
                    await roleManager.CreateAsync(new AppRole("admin"));
                    await roleManager.CreateAsync(new AppRole("user"));
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
                var systemUser = new AppUser
                {
                    UserName = "system",
                    Email = "system@system.com",
                    EmailConfirmed = true,
                    IsActive = true,
                    ActivatedBy = null,
                    ActivationDate = DateTime.UtcNow
                };

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
                            Type = Entities.Enums.UiConfigurationType.Dashboard,
                            Data = "{}",
                            UserId = 2,
                        },
                        new UiConfiguration
                        {
                            Type = Entities.Enums.UiConfigurationType.Control,
                            Data = "{}",
                            UserId = 2,
                        }
                    }
                };

                const string password = "admin1";

                _logger.LogInformation("Creating system user");
                await userManager.CreateAsync(systemUser, password);
                await userManager.AddToRoleAsync(systemUser, "admin");
                await userManager.AddToRoleAsync(systemUser, "user");

                _logger.LogInformation("Creating admin user");
                await userManager.CreateAsync(adminUser, password);
                await userManager.AddToRoleAsync(adminUser, "admin");
                await userManager.AddToRoleAsync(adminUser, "user");
            }
        }
    }
}