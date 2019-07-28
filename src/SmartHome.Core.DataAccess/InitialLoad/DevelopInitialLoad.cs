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
                            ContractAssembly = "SmartHome.Contracts.EspurnaMqtt.dll",
                            AssemblyProduct = "Espurna-MQTT-v1",
                            CreatedById = 1,
                            Created = DateTime.UtcNow,
                            RegisteredMagnitudes = new List<RegisteredMagnitude>
                            {
                                new RegisteredMagnitude { Magnitude = "generic_analog" },
                                new RegisteredMagnitude { Magnitude = "relay2" },
                                new RegisteredMagnitude { Magnitude = "relay1" },
                                new RegisteredMagnitude { Magnitude = "relay0" }
                            }
                        },
                        new ControlStrategy
                        {
                            Id = 2,
                            IsActive = true,
                            Description = "Control Espurna over REST",
                            ContractAssembly = "SmartHome.Contracts.EspurnaRest.dll",
                            AssemblyProduct = "Espurna-REST-v1",
                            CreatedById = 1,
                            Created = DateTime.UtcNow,
                            RegisteredMagnitudes = new List<RegisteredMagnitude>
                            {
                                new RegisteredMagnitude { Magnitude = "generic_analog" },
                                new RegisteredMagnitude { Magnitude = "relay2" },
                                new RegisteredMagnitude { Magnitude = "relay1" },
                                new RegisteredMagnitude { Magnitude = "relay0" }
                            }
                        },
                        new ControlStrategy
                        {
                            Id = 3,
                            IsActive = true,
                            Description = "Control generic devices over REST",
                            ContractAssembly = "SmartHome.Contracts.GenericRest.dll",
                            AssemblyProduct = "Generic-REST-v1",
                            CreatedById = 1,
                            Created = DateTime.UtcNow,
                            RegisteredMagnitudes = new List<RegisteredMagnitude>
                            {
                                new RegisteredMagnitude { Magnitude = "generic_analog" },
                                new RegisteredMagnitude { Magnitude = "relay2" },
                                new RegisteredMagnitude { Magnitude = "relay1" },
                                new RegisteredMagnitude { Magnitude = "relay0" }
                            }
                        },
                        new ControlStrategy
                        {
                            Id = 4,
                            IsActive = true,
                            Description = "Control Tasmota Socket with power measurment",
                            ContractAssembly = "SmartHome.Contracts.TasmotaMqtt.dll",
                            AssemblyProduct = "Tasmota-MQTT-v1",
                            CreatedById = 1,
                            Created = DateTime.UtcNow,
                            RegisteredMagnitudes = new List<RegisteredMagnitude>
                            {
                                new RegisteredMagnitude { Magnitude = "relay0" },
                                new RegisteredMagnitude { Magnitude = "voltage" },
                                new RegisteredMagnitude { Magnitude = "current" },
                                new RegisteredMagnitude { Magnitude = "power_active" },
                                new RegisteredMagnitude { Magnitude = "power_reactive" },
                                new RegisteredMagnitude { Magnitude = "power_apparent" },
                                new RegisteredMagnitude { Magnitude = "power_factor"}
                            }
                        },
                        new ControlStrategy
                        {
                            Id = 5,
                            IsActive = true,
                            Description = "Control Tasmota Socket with power measurment",
                            ContractAssembly = "SmartHome.Contracts.TasmotaMqtt.dll",
                            AssemblyProduct = "Tasmota-MQTT-v1",
                            CreatedById = 1,
                            Created = DateTime.UtcNow,
                            RegisteredMagnitudes = new List<RegisteredMagnitude>
                            {
                                new RegisteredMagnitude { Magnitude = "light" }
                            }
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
                    context.Add(new AppUserNodeLink
                    {
                        NodeId = createdNode2.Entity.Id,
                        UserId = 1
                    });
                    await context.SaveChangesAsync();

                    var node3 = new Node
                    {
                        Name = "Blitzwolf Socket Tasmota",
                        ControlStrategyId = 4,
                        Created = DateTime.UtcNow,
                        CreatedById = 1,
                        IpAddress = "http://192.168.0.220",
                        Port = 80,
                        GatewayIpAddress = null,
                        Description = "Blitzwolf Socket Tasmota",
                        ApiKey = null,
                        ClientId = "DVES_2323B9",
                        BaseTopic = "cmnd/sonoff/blitzwolf0"
                    };

                    var createdNode3 = await context.Nodes.AddAsync(node3);
                    await context.SaveChangesAsync();


                    context.Add(new AppUserNodeLink
                    {
                        NodeId = createdNode3.Entity.Id,
                        UserId = 1
                    });


                    var node4 = new Node
                    {
                        Name = "Sonoff B1 Light",
                        ControlStrategyId = 5,
                        Created = DateTime.UtcNow,
                        CreatedById = 1,
                        IpAddress = "http://192.168.0.221",
                        Port = 80,
                        GatewayIpAddress = null,
                        Description = "Sonoff B1 Light Tasmota",
                        ApiKey = null,
                        ClientId = "DVES_47C631",
                        BaseTopic = "cmnd/sonoff/sonoffb1"
                    };

                    var createdNode4 = await context.Nodes.AddAsync(node4);
                    await context.SaveChangesAsync();


                    context.Add(new AppUserNodeLink
                    {
                        NodeId = createdNode4.Entity.Id,
                        UserId = 1
                    });
                }
            }
        }
    }
}