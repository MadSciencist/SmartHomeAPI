using Autofac;
using SmartHome.Core.Entities.Entity;

namespace SmartHome.Core.Data.Repository
{
    public class AppUserNodeLinkRepository : GenericRepository<AppUserNodeLink>, IAppUserNodeLinkRepository
    {
        public AppUserNodeLinkRepository(ILifetimeScope container) : base(container)
        {
        }
    }
}