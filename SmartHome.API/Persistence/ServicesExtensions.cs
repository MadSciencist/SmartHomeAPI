using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SmartHome.API.Persistence.App;
using SmartHome.API.Repository;
using SmartHome.Repositories;

namespace SmartHome.API.Persistence
{
    public static class ServicesExtensions
    {
        public static void RegisterRepositoriesToIocContainer(this IServiceCollection services)
        {
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<INodeRepository, NodeRepository>();
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
    }
}