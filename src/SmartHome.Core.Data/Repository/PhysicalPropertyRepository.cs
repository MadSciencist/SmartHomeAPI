using Autofac;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Repositories;

namespace SmartHome.Core.Data.Repository
{
    public class PhysicalPropertyRepository : GenericRepository<PhysicalProperty, int>, IPhysicalPropertyRepository
    {
        public PhysicalPropertyRepository(ILifetimeScope container) : base(container)
        {
        }
    }
}
