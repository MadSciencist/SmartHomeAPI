using Microsoft.Extensions.DependencyInjection;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                            Description = "Control ESPURNA device over HTTP and REST",
                            ControlProviderName = "Rest",
                            ControlContext = "Espurna",
                            ReceiveProviderName = "Mqtt",
                            ReceiveContext = "EspurnaJsonPayload",
                            CreatedById = 1,
                            ControlStrategyLinkages = new List<ControlStrategyLinkage>
                            {
                                new ControlStrategyLinkage
                                {
                                    ControlStrategyId = 2,
                                    ControlStrategyLinkageTypeId = (int)ELinkageType.Sensor,
                                    InternalValue = "analog",
                                    DisplayValue = "Generic Analog"
                                },
                                new ControlStrategyLinkage
                                {
                                    ControlStrategyId = 2,
                                    ControlStrategyLinkageTypeId = (int)ELinkageType.Command,
                                    InternalValue = "ToggleRelay",
                                    DisplayValue = "Toggle Relay"
                                },
                                new ControlStrategyLinkage
                                {
                                    ControlStrategyId = 2,
                                    ControlStrategyLinkageTypeId = (int)ELinkageType.Sensor,
                                    InternalValue = "relay/0", // relay still might be sensor (subscribe to changes)
                                    DisplayValue = "Relay 0"
                                }
                            },
                            Created = DateTime.UtcNow
                        },
                        new ControlStrategy
                        {
                            Id = 2,
                            IsActive = true,
                            Description = "Control over MQTT",
                            ControlProviderName = "Rest",
                            ControlContext = "Espurna",
                            ReceiveProviderName = "Mqtt",
                            ReceiveContext = "Espurna",
                            ControlStrategyLinkages = new List<ControlStrategyLinkage>
                            {
                                new ControlStrategyLinkage
                                {
                                    ControlStrategyId = 2,
                                    ControlStrategyLinkageTypeId = (int)ELinkageType.Sensor,
                                    InternalValue = "analog",
                                    DisplayValue = "Generic Analog"
                                },
                                new ControlStrategyLinkage
                                {
                                    ControlStrategyId = 2,
                                    ControlStrategyLinkageTypeId = (int)ELinkageType.Command,
                                    InternalValue = "ToggleRelay",
                                    DisplayValue = "Toggle Relay"
                                },
                                new ControlStrategyLinkage
                                {
                                    ControlStrategyId = 2,
                                    ControlStrategyLinkageTypeId = (int)ELinkageType.Sensor,
                                    InternalValue = "relay/0", // relay still might be sensor (subscribe to changes)
                                    DisplayValue = "Relay 0"
                                }
                            },
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
                        IpAddress = "http://192.168.0.101",
                        Port = 80,
                        GatewayIpAddress = "http://192.168.0.1",
                        Description = "Dev test node",
                        ApiKey = "03102E55CD7BBE35",
                        ClientId = "clientId"
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
                        ControlStrategyId = 2,
                        Created = DateTime.UtcNow,
                        CreatedById = 1,
                        IpAddress = "http://192.168.0.100",
                        Port = 80,
                        GatewayIpAddress = "http://192.168.0.1",
                        Description = "Dev test node",
                        ApiKey = "03102E55CD7BBE35",
                        ClientId = "clientId100"
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