using Autofac;
using SmartHome.Core.Entities.Entity;

namespace SmartHome.Core.Data.Repository
{
    public class StrategyRepository : GenericRepository<ControlStrategy>, IStrategyRepository
    {
        public StrategyRepository(ILifetimeScope container) : base(container)
        {
        }
    }
}