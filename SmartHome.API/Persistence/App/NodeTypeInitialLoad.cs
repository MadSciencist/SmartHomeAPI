using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartHome.Domain.Entity;
using SmartHome.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.API.Persistence.App
{
    public static class NodeTypeInitialLoad
    {
        public static async Task Seed(IServiceProvider provider)
        {
            using (IServiceScope scope = provider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
                    .CreateLogger(nameof(NodeTypeInitialLoad));
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var nodeTypes = new List<NodeType>()
                {
                    new NodeType()
                    {
                        Id = 1,
                        Name = "device",
                        Description = "Node which can be controlled"
                    },
                    new NodeType()
                    {
                        Id = 2,
                        Name = "sensor",
                        Description = "Node which sends data"
                    },
                };

                if (!context.NodeTypes.Any())
                {
                    logger.LogInformation("Truncating node_type table");
                    await context.Database.ExecuteSqlCommandAsync("SET FOREIGN_KEY_CHECKS = 0;TRUNCATE TABLE node_type;SET FOREIGN_KEY_CHECKS = 1;");

                    foreach (var nodeType in nodeTypes)
                    {
                        logger.LogInformation("Loading node types into node_type table");
                        await context.AddAsync(nodeType);
                    }

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
