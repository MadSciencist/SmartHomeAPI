using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace SmartHome.Core.DataAccess
{
    /// <summary>
    /// Used by EF CLI
    /// </summary>
    public class DbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            //TODO
            //IConfigurationRoot configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json")
            //    .Build();

            //  var connectionString = configuration["ConnectionStrings:MySql"];

            var connectionString = @"server=localhost;port=3306;userid=root;password=;database=smarthomedb;SslMode=none;CharSet=utf8";

            var builder = new DbContextOptionsBuilder<AppDbContext>();

            builder.UseMySql(connectionString, mySqlOptions =>
                {
                    mySqlOptions.ServerVersion(new Version(10, 1, 38), ServerType.MariaDb);
                    mySqlOptions.MigrationsHistoryTable("__MigrationHistory");
                })
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();

            return new AppDbContext(builder.Options);
        }
    }
}