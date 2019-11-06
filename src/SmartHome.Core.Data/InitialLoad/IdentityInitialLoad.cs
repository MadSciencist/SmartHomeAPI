using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Entities.Enums;
using SmartHome.Core.Entities.Role;
using SmartHome.Core.Entities.User;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
            using var scope = _provider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

            if (!await roleManager.RoleExistsAsync(Roles.Admin)
                && !await roleManager.RoleExistsAsync(Roles.User))
            {
                _logger.LogInformation("Creating roles...");

                var adminRole = new AppRole(Roles.Admin);
                var userRole = new AppRole(Roles.User);

                await roleManager.CreateAsync(adminRole);
                await roleManager.CreateAsync(userRole);

                await roleManager.AddClaimAsync(adminRole, new Claim(ClaimTypes.Role, Roles.Admin));
                await roleManager.AddClaimAsync(userRole, new Claim(ClaimTypes.Role, Roles.User));

                _logger.LogInformation("Roles created!");
            }
        }

        public async Task SeedUsers()
        {
            using var scope = _provider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            if (await userManager.FindByNameAsync("admin") != null) return;

            // id = 1
            var systemUser = new AppUser
            {
                UserName = "system",
                Email = "test@system.com",
                EmailConfirmed = true,
                IsActive = true,
                ActivatedBy = null,
                ActivationDate = DateTime.UtcNow
            };

            // id = 2
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
                        Type = UiConfigurationType.Page,
                        Name = "Home Page",
                        Data = "{}",
                        UserId = 2,
                    }
                }
            };

            // id = 3
            var testUser = new AppUser
            {
                UserName = "test",
                Email = "system@system.com",
                EmailConfirmed = true,
                IsActive = true,
                ActivatedBy = null,
                ActivationDate = DateTime.UtcNow
            };

            await CreateAsync(userManager, systemUser, new List<string> { Roles.Admin });
            await CreateAsync(userManager, adminUser, new List<string> { Roles.Admin });
            await CreateAsync(userManager, testUser, new List<string> { Roles.User });
        }

        private async Task CreateAsync(UserManager<AppUser> userManager, AppUser user, List<string> roles)
        {
            const string password = "admin1"; // TODO to user secrets

            _logger.LogInformation($"Creating {user.UserName} user...");

            await userManager.CreateAsync(user, password);
            await userManager.AddToRolesAsync(user, roles);
            await AddDefaultClaimsAsync(userManager, user, roles);

            _logger.LogInformation("User created!");
        }

        private static async Task AddDefaultClaimsAsync(UserManager<AppUser> userManager, AppUser user, List<string> roles)
        {
            var defaultClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            await userManager.AddClaimsAsync(user, defaultClaims);
        }
    }
}