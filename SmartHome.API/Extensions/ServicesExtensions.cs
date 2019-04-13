﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SmartHome.API.Persistence.App;
using SmartHome.API.Persistence.Identity;
using SmartHome.API.Repository;
using SmartHome.API.Security.Token;
using SmartHome.API.Services.Crud;
using SmartHome.Domain.Role;
using SmartHome.Domain.User;
using SmartHome.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using SmartHome.DeviceController;
using SmartHome.DeviceController.Mqtt;
using SmartHome.DeviceController.Rest;
using Swashbuckle.AspNetCore.Swagger;

namespace SmartHome.API.Extensions
{
    public static class ServicesExtensions
    {
        public static void RegisterAppServicesToIocContainer(this IServiceCollection services)
        {
            services.AddTransient<ITokenBuilder, TokenBuilder>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<INodeRepository, NodeRepository>();
            services.AddTransient<ICrudNodeService, CrudNodeService>();
        }

        public static void RegisterAppServicesToAutofacContainer(this ContainerBuilder builder)
        {
            builder.RegisterType<RestControlStrategy>()
                .Named<IControlStrategy>("SmartHome.DeviceController.Rest.RestControlStrategy");
            builder.RegisterType<MqttControlStrategy>()
                .Named<IControlStrategy>("SmartHome.DeviceController.Mqtt.MqttControlStrategy");
        }

        public static void AddSqlPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["ConnectionStrings:MySql"];

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(connectionString, mysqlOptions =>
                    {
                        mysqlOptions.ServerVersion(new Version(10, 1, 29), ServerType.MariaDb)
                            .MigrationsHistoryTable("__AppMigrationHistory");
                    })
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
                // .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }

        public static void AddSqlIdentityPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["ConnectionStrings:MySql"];

            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseMySql(connectionString, mySqlOptions =>
                {
                    mySqlOptions.ServerVersion(new Version(10, 1, 38), ServerType.MariaDb);
                    mySqlOptions.MigrationsHistoryTable("__IdentityMigrationHistory");
                });
            });

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
                options.AddPolicy("Admin", policy => policy.RequireClaim("admin"));
                options.AddPolicy("User", policy => policy.RequireClaim("user"));
                options.AddPolicy("Sensor", policy => policy.RequireClaim("sensor"));
            });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
            });
        }

        public static void AddDefaultCorsPolicy(this IServiceCollection services)
        {
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
                    {"Bearer",new string[]{}}
                });
            });
        }
    }
}
