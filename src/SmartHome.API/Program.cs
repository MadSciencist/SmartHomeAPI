using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Core.DataAccess.InitialLoad;

namespace SmartHome.API
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("webapp.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: null)
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");

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
                    //var env = hostContext.HostingEnvironment;
                    //if (env.IsDevelopment())
                    //    config.AddUserSecrets();
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
                    options.Listen(IPAddress.Any, 5000);
                });

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
