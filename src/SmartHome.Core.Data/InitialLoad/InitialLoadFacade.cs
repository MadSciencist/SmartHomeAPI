using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace SmartHome.Core.Data.InitialLoad
{
    public class InitialLoadFacade
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<InitialLoadFacade> _logger;

        public InitialLoadFacade(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetRequiredService<ILogger<InitialLoadFacade>>();
        }

        public async Task Seed()
        {
            try
            {
                EnsureDatabaseWithLatestSchemaExists();

                var identitySeeder = new IdentityInitialLoad(_serviceProvider);
                await identitySeeder.SeedRoles();
                await identitySeeder.SeedUsers();

                await DictionaryInitialLoad.Seed(_serviceProvider);
                await ConfigInitialLoad.Seed(_serviceProvider);
                await DevelopInitialLoad.Seed(_serviceProvider);
            }
            catch (Exception ex)
                when (ex is AggregateException
                      && ex.InnerException is InvalidOperationException iox
                      && iox.InnerException is MySqlException mysqlEx)
            {
                _logger.LogError("Cannot seed database, MySQL-level exception. DB server might be off.", mysqlEx);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Cannot seed database, DB server might be off.", ex);
                throw;
            }
        }

        private void EnsureDatabaseWithLatestSchemaExists()
        {
            var dbContext = _serviceProvider.GetRequiredService<EntityFrameworkContext>();
            dbContext.Database.Migrate();
        }
    }
}
