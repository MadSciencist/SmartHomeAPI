using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SmartHome.API.Security;
using SmartHome.API.Service;
using SmartHome.Core.Data;
using SmartHome.Core.Entities.Enums;
using SmartHome.Core.Entities.Role;
using SmartHome.Core.Entities.User;
using SmartHome.Core.Security;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SmartHome.API.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddApiServices(this IServiceCollection services)
        {
            services.AddTransient<ITokenBuilder, JwtTokenBuilder>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<HubTokenDecoder>();
        }

        public static void AddSqlIdentityPersistence(this IServiceCollection services, IConfiguration configuration, IHostingEnvironment env)
        {
            var connectionString = configuration["ConnectionStrings:MySql"];

            services.AddDbContext<EntityFrameworkContext>(options =>
            {
                options.UseMySql(connectionString, mySqlOptions =>
                {
                    mySqlOptions.ServerVersion(new Version(10, 1, 38), ServerType.MariaDb);
                    mySqlOptions.MigrationsHistoryTable("__migrationHistory");
                });

                if (env.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }
            });

            services.AddIdentity<AppUser, AppRole>(options =>
                {
                    options.Password.RequiredLength = 5;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = false;
                })
                .AddEntityFrameworkStores<EntityFrameworkContext>()
                .AddDefaultTokenProviders()
                .AddRoles<AppRole>();
        }

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
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
                options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, Roles.Admin));
                options.AddPolicy("User", policy => policy.RequireClaim(ClaimTypes.Role, Roles.User));
            });
        }

        public static void AddDefaultCorsPolicy(this IServiceCollection services, IHostingEnvironment env)
        {
            if (!env.IsDevelopment()) return;
            services.AddCors(settings =>
            {
                settings.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });
        }

        public static void AddConfiguredSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
            {
                var contact = new Contact { Email = "mkrysz1337@gmail.com", Name = "Matty" };
                swagger.SwaggerDoc("dev", new Info { Title = "Home Sensor Server API", Version = "v1", Contact = contact });
                swagger.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                swagger.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[]{}}
                });
            });
        }
    }
}
