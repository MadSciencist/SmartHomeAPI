using System;
using System.Threading.Tasks;

namespace SmartHome.Core.Data.InitialLoad
{
    public class InitialLoadFacade
    {
        private readonly IServiceProvider _serviceProvider;

        public InitialLoadFacade(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Seed()
        {
            var identitySeeder = new IdentityInitialLoad(_serviceProvider);
            await identitySeeder.SeedRoles();
            await identitySeeder.SeedUsers();

            await DictionaryInitialLoad.Seed(_serviceProvider);
            await ConfigInitialLoad.Seed(_serviceProvider);
            await DevelopInitialLoad.Seed(_serviceProvider);
        }
    }
}
