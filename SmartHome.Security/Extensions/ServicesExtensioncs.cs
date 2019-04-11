using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SmartHome.Security.Persistence;
using System;
using System.Text;
using SmartHome.Domain.Role;
using SmartHome.Domain.User;
using SmartHome.Security.Token;

namespace SmartHome.Security.Extensions
{
    public static class ServicesExtensions
    {
        public static void RegisterIdentityToIocContainer(this IServiceCollection services)
        {
            services.AddTransient<ITokenBuilder, TokenBuilder>();
        }

        public static void AddSqlIdentityPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["ConnectionStrings:MySql"];

            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseMySql(connectionString, mySqlOptions =>
                    {
                        mySqlOptions.ServerVersion(new Version(10, 1, 38), ServerType.MariaDb);
                    });
            });
        }

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 5; // Sample validator
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders()
            .AddRoles<AppRole>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    SaveSigninToken = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.FromMinutes(0)
                });
        }

        public static void AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("admin"));
                options.AddPolicy("User", policy => policy.RequireClaim("user"));
                options.AddPolicy("Sensor", policy => policy.RequireClaim("sensor"));
            });
        }
    }
}
