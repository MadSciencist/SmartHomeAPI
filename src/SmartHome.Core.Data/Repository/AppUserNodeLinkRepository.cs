using Autofac;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Repositories;

namespace SmartHome.Core.Data.Repository
{
    public class AppUserNodeLinkRepository : GenericRepository<AppUserNodeLink, int>, IAppUserNodeLinkRepository
    {
        public AppUserNodeLinkRepository(ILifetimeScope container) : base(container)
        {
        }
    }
}