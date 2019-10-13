using Autofac;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Entities.Entity;
using System.Linq;

namespace SmartHome.Core.DataAccess.Repository
{
    public class StrategyRepository : GenericRepository<ControlStrategy>, IStrategyRepository
    {
        public StrategyRepository(ILifetimeScope container) : base(container)
        {
        }

        public override IQueryable<ControlStrategy> AsQueryableNoTrack()
        {
            return base.AsQueryableNoTrack().Include(x => x.RegisteredMagnitudes);
        }
    }
}