using Microsoft.Extensions.DependencyInjection;
using SmartHome.Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.InitialLoad
{
    public static class DevelopInitialLoad
    {
        public static async Task Seed(IServiceProvider provider)
        {
            using (IServiceScope scope = provider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (!context.ControlStrategies.Any())
                {
                    var controlStrategies = new List<ControlStrategy>
                    {
                        new ControlStrategy
                        {
                            Id = 1,
                            IsActive = true,
                            Description = "Control Espurna over MQTT",
                            ContractAssembly = "SmartHome.Contracts.EspurnaMqtt",
                            CreatedById = 1,
                            Created = DateTime.UtcNow
                        },
                        new ControlStrategy
                        {
                            Id = 2,
                            IsActive = true,
                            Description = "Control Espurna over REST",
                            ContractAssembly = "SmartHome.Contracts.EspurnaRest",
                            CreatedById = 1,
                            Created = DateTime.UtcNow
                        }
                    };

                    await context.AddRangeAsync(controlStrategies);
                    await context.SaveChangesAsync();
                }

                if (!context.Nodes.Any(x => x.Name == "Dev"))
                {
                    var node = new Node
                    {
                        Name = "Dev",
                        ControlStrategyId = 1,
                        Created = DateTime.UtcNow,
                        CreatedById = 1,
                        IpAddress = "http://192.168.0.211",
                        Port = 80,
                        GatewayIpAddress = "http://192.168.0.1",
                        Description = "Dev test node",
                        ApiKey = "03102E55CD7BBE35",
                        ClientId = "clientId",
                        BaseTopic = "root"
                    };

                    var createdNode = await context.Nodes.AddAsync(node);
                    await context.SaveChangesAsync();

                    context.Add(new AppUserNodeLink
                    {
                        NodeId = createdNode.Entity.Id,
                        UserId = 1
                    });

                    var node2 = new Node
                    {
                        Name = "Dev2",
                        ControlStrategyId = 1,
                        Created = DateTime.UtcNow,
                        CreatedById = 1,
                        IpAddress = "http://192.168.0.210",
                        Port = 80,
                        GatewayIpAddress = "http://192.168.0.1",
                        Description = "Dev test node",
                        ApiKey = "03102E55CD7BBE35",
                        ClientId = "clientId100",
                        BaseTopic = "root"
                    };

                    var createdNode2 = await context.Nodes.AddAsync(node2);
                    await context.SaveChangesAsync();


                    context.Add(new AppUserNodeLink
                    {
                        NodeId = createdNode2.Entity.Id,
                        UserId = 1
                    });

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}