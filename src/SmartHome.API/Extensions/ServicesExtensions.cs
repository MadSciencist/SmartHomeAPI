using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SmartHome.API.Security;
using SmartHome.API.Service;
using SmartHome.Core.Data;
using SmartHome.Core.Entities.Enums;
using SmartHome.Core.Entities.Role;
using SmartHome.Core.Entities.User;
using SmartHome.Core.Security;
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

        public static void AddSqlIdentityPersistence(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            var connectionString = configuration["ConnectionStrings:MySql"];

            services.AddDbContext<EntityFrameworkContext>(options =>
            {
                options.UseMySql(connectionString, mySqlOptions =>
                {
                    mySqlOptions.ServerVersion(new Version(10, 1, 38), ServerType.MariaDb);
                    mySqlOptions.MigrationsHistoryTable("__migrationHistory");
                });

                if (!env.IsDevelopment()) return;

                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }, ServiceLifetime.Transient);

            services.AddIdentity<AppUser, AppRole>(options =>
                {
                    options.Password.RequiredLength = 5;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = false;
                })
                .AddRoleManager<RoleManager<AppRole>>()
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
                options.AddPolicy(Roles.Admin, policy => policy.RequireClaim(ClaimTypes.Role, Roles.Admin));
                
                // User policy is also valid for admin role, so we don't have to explicitly specify two roles
                options.AddPolicy(Roles.User,
                    policy =>
                    {
                        policy.RequireAssertion(context => 
                            context.User.HasClaim(c => 
                                c.Type == ClaimTypes.Role && (c.Value == Roles.User || c.Type == Roles.Admin)));
                    });
            });
        }

        public static void AddDefaultCorsPolicy(this IServiceCollection services, IWebHostEnvironment env)
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
            services.AddSwaggerGen(options =>
            {
                var contact = new OpenApiContact { Email = "mkrysz1337@gmail.com", Name = "Matty" };
                options.SwaggerDoc("v1", new OpenApiInfo {Title = "Home Sensor Server API", Version = "v1", Contact = contact});

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}
