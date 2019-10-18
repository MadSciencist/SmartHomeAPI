using Autofac;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Repositories;

namespace SmartHome.Core.Data.Repository
{
    public class StrategyRepository : GenericRepository<ControlStrategy, int>, IStrategyRepository
    {
        public StrategyRepository(ILifetimeScope container) : base(container)
        {
        }
    }
}