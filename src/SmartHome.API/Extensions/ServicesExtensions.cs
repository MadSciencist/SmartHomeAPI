using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SmartHome.API.Security.Token;
using SmartHome.Core.DataAccess;
using SmartHome.Core.Domain.Role;
using SmartHome.Core.Domain.User;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using SmartHome.API.Hubs;
using SmartHome.Core.Domain.Enums;

namespace SmartHome.API.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddApiServices(this IServiceCollection services)
        {
            services.AddTransient<ITokenBuilder, JwtTokenBuilder>();
            services.AddSingleton<HubNotifier>();
            services.AddScoped<HubTokenDecoder>();
        }

        public static void AddSqlIdentityPersistence(this IServiceCollection services, IConfiguration configuration, IHostingEnvironment env)
        {
            var connectionString = configuration["ConnectionStrings:MySql"];

            services.AddDbContext<AppDbContext>(options =>
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
                .AddEntityFrameworkStores<AppDbContext>()
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
                options.AddPolicy("Sensor", policy => policy.RequireClaim(ClaimTypes.Role, Roles.Sensor));
            });
        }

        public static void AddDefaultCorsPolicy(this IServiceCollection services, IHostingEnvironment env)
        {
            //if (!env.IsDevelopment()) return;
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
                swagger.SwaggerDoc("dev", new Info { Title = "Home Sensor Server API", Version = "v1", Contact = new Contact { Email = "mkrysz1337@gmail.com" } });
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
