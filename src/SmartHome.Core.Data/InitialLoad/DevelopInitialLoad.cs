using Microsoft.Extensions.DependencyInjection;
using SmartHome.Core.Entities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Data.InitialLoad
{
    public static class DevelopInitialLoad
    {
        public static async Task Seed(IServiceProvider provider)
        {
            using (IServiceScope scope = provider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<EntityFrameworkContext>();

                if (!context.ControlStrategies.Any())
                {
                    var controlStrategies = new List<ControlStrategy>
                    {
                        new ControlStrategy
                        {
                            Id = 4,
                            IsActive = true,
                            Name = "Control Tasmota Socket with power measurement",
                            Description = "Control Tasmota Socket with power measurement",
                            ContractAssembly = "SmartHome.Contracts.TasmotaMqtt.dll",
                            Connector = "Tasmota-MQTT-v1",
                            CreatedById = 1,
                            Created = DateTime.UtcNow,
                            PhysicalProperties = new List<PhysicalPropertyControlStrategyLink>
                            {
                                new PhysicalPropertyControlStrategyLink  { ControlStrategyId = 4, PhysicalPropertyId = 103 }, // Current
                                new PhysicalPropertyControlStrategyLink  { ControlStrategyId = 4, PhysicalPropertyId = 104 }, // Voltage
                                new PhysicalPropertyControlStrategyLink  { ControlStrategyId = 4, PhysicalPropertyId = 105 }, // Active power
                                new PhysicalPropertyControlStrategyLink  { ControlStrategyId = 4, PhysicalPropertyId = 106 }, // Apparent Power
                                new PhysicalPropertyControlStrategyLink  { ControlStrategyId = 4, PhysicalPropertyId = 107 }, // Reactive power
                                new PhysicalPropertyControlStrategyLink  { ControlStrategyId = 4, PhysicalPropertyId = 108 }, // Power factor
                                new PhysicalPropertyControlStrategyLink  { ControlStrategyId = 4, PhysicalPropertyId = 109 }, // Relay #1
                            }
                        },
                        new ControlStrategy
                        {
                            Id = 5,
                            IsActive = true,
                            Name = "Control Tasmota Light",
                            Description = "Control Tasmota Light",
                            ContractAssembly = "SmartHome.Contracts.TasmotaMqtt.dll",
                            Connector = "Tasmota-MQTT-v1",
                            CreatedById = 1,
                            Created = DateTime.UtcNow,
                            PhysicalProperties = new List<PhysicalPropertyControlStrategyLink>
                            {
                                new PhysicalPropertyControlStrategyLink  { ControlStrategyId = 4, PhysicalPropertyId = 300 }, // Light
                            }
                        }
                    };

                    await context.AddRangeAsync(controlStrategies);
                    await context.SaveChangesAsync();
                }

                if (!context.Nodes.Any())
                {
                    var node1 = new Node
                    {
                        Name = "BlitzWolf Socket Tasmota",
                        ControlStrategyId = 4,
                        Created = DateTime.UtcNow,
                        CreatedById = 1,
                        UriSchema = "http://",
                        IpAddress = "192.168.0.220",
                        Port = 80,
                        GatewayIpAddress = null,
                        Description = "Blitzwolf Socket Tasmota",
                        ApiKey = null,
                        ClientId = "DVES_2323B9",
                        BaseTopic = "cmnd/sonoff/blitzwolf0",
                    };

                    var createdNode1 = await context.Nodes.AddAsync(node1);
                    await context.SaveChangesAsync();

                    context.Add(new AppUserNodeLink
                    {
                        NodeId = createdNode1.Entity.Id,
                        UserId = 1
                    });

                    var node2 = new Node
                    {
                        Name = "Sonoff B1 Light",
                        ControlStrategyId = 4,
                        Created = DateTime.UtcNow,
                        CreatedById = 1,
                        UriSchema = "http://",
                        IpAddress = "192.168.0.221",
                        Port = 80,
                        GatewayIpAddress = null,
                        Description = "Sonoff B1 Light Tasmota",
                        ApiKey = null,
                        ClientId = "DVES_47C631",
                        BaseTopic = "cmnd/sonoff/sonoffb1"
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