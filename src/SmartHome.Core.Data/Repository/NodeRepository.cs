using Autofac;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Data.Repository
{
    public class NodeRepository : GenericRepository<Node, int>, INodeRepository
    {
        public NodeRepository(ILifetimeScope container) : base(container)
        {
        }

        public override async Task<IEnumerable<Node>> GetAllAsync()
        {

            var lastSeens = (await Context.NodeData
                .Select(x => new { x.NodeId, x.TimeStamp })
                .ToListAsync()
                ).GroupBy(x => x.NodeId, (NodeId, TimeStamps) => // TODO: evaluate on DB for performance (ef 3.0 doesn't support it natively)
                new NodeLastSeenDto
                {
                    NodeId = NodeId,
                    LastSeen = TimeStamps.Max(x => x.TimeStamp)
                });

            var nodes = await Context.Nodes
                .Include(x => x.ControlStrategy)
                    .ThenInclude(x => x.PhysicalProperties)
                        .ThenInclude(x => x.PhysicalProperty)
                .Include(x => x.CreatedBy)
                .Include(x => x.AllowedUsers)
                .ToListAsync();

            foreach(var node in nodes)
            {
                node.LastSeen = lastSeens.FirstOrDefault(x => x.NodeId == node.Id).LastSeen;
            }

            return nodes;
        }

        public async Task<IEnumerable<string>> GetAllClientIdsAsync()
        {
            return await Context.Nodes
                .Select(x => x.ClientId)
                .ToListAsync();
        }

        public override async Task<Node> GetByIdAsync(int id)
        {
            return await Context.Nodes
                .Include(x => x.ControlStrategy)
                    .ThenInclude(x => x.PhysicalProperties)
                        .ThenInclude(x => x.PhysicalProperty)
                .Include(x => x.CreatedBy)
                .Include(x => x.AllowedUsers)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Node> GetByClientIdAsync(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                return null;
            }

            return await Context.Nodes
                .Include(x => x.ControlStrategy)
                    .ThenInclude(x => x.PhysicalProperties)
                        .ThenInclude(x => x.PhysicalProperty)
                .Include(x => x.CreatedBy)
                .Include(x => x.AllowedUsers)
                .FirstOrDefaultAsync(x => x.ClientId == clientId);
        }
    }
}