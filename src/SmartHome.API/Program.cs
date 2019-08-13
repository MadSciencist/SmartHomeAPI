using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SmartHome.Core.DataAccess.InitialLoad;
using System;

namespace SmartHome.API
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                var host = CreateWebHostBuilder(args).Build();
                InitializeDatabase(host.Services);
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    SetupLogger(hostContext.HostingEnvironment.EnvironmentName);
                    config.AddUserSecrets<Startup>();
                })
                .ConfigureLogging(logger =>
                {
                    logger.ClearProviders();
                    logger.AddSerilog();
                })
                .UseKestrel(options =>
                {
                    options.Limits.MaxConcurrentConnections = 100;
                    options.Limits.MaxConcurrentUpgradedConnections = 100;
                });

        private static void SetupLogger(string env)
        {
            var config = env == "Development" ? "appsettings.Development.json" : "appsettings.json";
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(config)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
                .CreateLogger();
        }

        private static void InitializeDatabase(IServiceProvider services)
        {
            using (var scope = services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var initialLoadFacade = new InitialLoadFacade(scope.ServiceProvider);
                initialLoadFacade.Seed().Wait();
            }
        }
    }
}
