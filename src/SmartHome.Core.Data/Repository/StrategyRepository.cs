using Autofac;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Data.Repository
{
    public class StrategyRepository : GenericRepository<ControlStrategy, int>, IControlStrategyRepository
    {
        public StrategyRepository(ILifetimeScope container) : base(container)
        {
        }

        public override async Task<ControlStrategy> GetByIdAsync(int id)
        {
            return await Context.ControlStrategies
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .Include(x => x.Nodes)
                .Include(x => x.PhysicalProperties)
                    .ThenInclude(x => x.PhysicalProperty)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<IEnumerable<ControlStrategy>> GetAllAsync()
        {
            return await Context.ControlStrategies
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .Include(x => x.Nodes)
                .Include(x => x.PhysicalProperties)
                    .ThenInclude(x => x.PhysicalProperty)
                .ToListAsync();
        }

        public async Task<ControlStrategy> CreateWithPropertyLinksAsync(ControlStrategy strategy, IEnumerable<int> physicalPropertyIds)
        {
            SetCreationAudit(strategy);

            await Context.AddAsync(strategy);

            if (physicalPropertyIds != null && physicalPropertyIds.Count() > 0)
            {
                strategy.PhysicalProperties = physicalPropertyIds.Select(id => new PhysicalPropertyControlStrategyLink
                {
                    ControlStrategyId = strategy.Id,
                    PhysicalPropertyId = id
                })
                .ToList();
            }

            return await base.CreateAsync(strategy);
        }

        public async Task<ControlStrategy> UpdateWithLinksAsync(ControlStrategy entity, IEnumerable<PhysicalPropertyControlStrategyLink> links)
        {
            var physicalPropertyControlStrategyLinks = links.ToList();

            foreach (var link in physicalPropertyControlStrategyLinks)
            {
                // Check if there is a new link request, if so add it
                if (!entity.PhysicalProperties.Any(x => x.PhysicalPropertyId == link.Id))
                {
                    var newLink = new PhysicalPropertyControlStrategyLink
                    {
                        ControlStrategyId = entity.Id,
                        PhysicalPropertyId = link.PhysicalPropertyId
                    };

                    Context.Entry(newLink).State = EntityState.Added;
                }
            }

            // Remove links that are currently in entity, but not present in links
            foreach (var existingLink in entity.PhysicalProperties)
            {
                if (!physicalPropertyControlStrategyLinks.Any(x => x.Id == existingLink.PhysicalPropertyId))
                {
                    Context.Entry(existingLink).State = EntityState.Deleted;
                }
            }

            return await base.UpdateAsync(entity);
        }
    }
}