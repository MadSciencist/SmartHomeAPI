﻿using Microsoft.Extensions.DependencyInjection;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
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
                            Description = "Control Espurna over REST and receive using MQTT and JSON payload",
                            ControlProviderName = "Rest",
                            ControlContext = "Espurna",
                            ReceiveProviderName = "Mqtt",
                            ReceiveContext = "EspurnaJsonPayload",
                            CreatedById = 1,
                            ControlStrategyLinkages = new List<ControlStrategyLinkage>
                            {
                                new ControlStrategyLinkage
                                {
                                    ControlStrategyId = 1,
                                    ControlStrategyLinkageTypeId = (int)LinkageType.Sensor,
                                    InternalValue = "analog",
                                    DisplayValue = "Generic Analog"
                                },
                                new ControlStrategyLinkage
                                {
                                    ControlStrategyId = 1,
                                    ControlStrategyLinkageTypeId = (int)LinkageType.Command,
                                    InternalValue = "SingleRelay",
                                    DisplayValue = "SingleRelay"
                                }
                            },
                            Created = DateTime.UtcNow
                        },
                        new ControlStrategy
                        {
                            Id = 2,
                            IsActive = true,
                            Description = "Control Espurna over REST and receive using MQTT",
                            ControlProviderName = "Rest",
                            ControlContext = "Espurna",
                            ReceiveProviderName = "Mqtt",
                            ReceiveContext = "Espurna",
                            ControlStrategyLinkages = new List<ControlStrategyLinkage>
                            {
                                new ControlStrategyLinkage
                                {
                                    ControlStrategyId = 2,
                                    ControlStrategyLinkageTypeId = (int)LinkageType.Sensor,
                                    InternalValue = "analog",
                                    DisplayValue = "Generic Analog"
                                },
                                new ControlStrategyLinkage
                                {
                                    ControlStrategyId = 2,
                                    ControlStrategyLinkageTypeId = (int)LinkageType.Command,
                                    InternalValue = "SingleRelay",
                                    DisplayValue = "Single Relay"
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