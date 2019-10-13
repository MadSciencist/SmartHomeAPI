using Autofac;
using SmartHome.Core.Entities.Entity;

namespace SmartHome.Core.DataAccess.Repository
{
    public class NodeDataMagnitudeRepository : GenericRepository<NodeDataMagnitude>, INodeDataMagnitudeRepository
    {
        public NodeDataMagnitudeRepository(ILifetimeScope container) : base(container)
        {
        }
    }
}
