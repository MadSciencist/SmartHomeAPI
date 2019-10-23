using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.IO;

namespace SmartHome.Core.Data
{
    /// <summary>
    /// Used by EF CLI
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EntityFrameworkContext>
    {
        public EntityFrameworkContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), @"../SmartHome.API/"))
                .AddJsonFile("appsettings.json")
                .AddUserSecrets("001b4959-7f46-4591-8b22-121446ac4d8e")
                .Build();

            var connectionString = configuration["ConnectionStrings:MySql"];

            var builder = new DbContextOptionsBuilder<EntityFrameworkContext>()
                .UseMySql(connectionString, mySqlOptions =>
                {
                    mySqlOptions.ServerVersion(new Version(10, 1, 38), ServerType.MariaDb);
                    mySqlOptions.MigrationsHistoryTable("__MigrationHistory");
                })
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();

            return new EntityFrameworkContext(builder.Options);
        }
    }
}