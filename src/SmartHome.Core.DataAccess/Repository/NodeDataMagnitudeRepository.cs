using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Entities.Entity;

namespace SmartHome.Core.DataAccess.Repository
{
    public class NodeDataMagnitudeRepository : GenericRepository<NodeDataMagnitude>, INodeDataMagnitudeRepository
    {
        public NodeDataMagnitudeRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }
    }
}
