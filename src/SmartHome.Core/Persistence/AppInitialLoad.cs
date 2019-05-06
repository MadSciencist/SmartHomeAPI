using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Control.Mqtt;
using SmartHome.Core.Control.Rest;
using SmartHome.Core.Persistence;
using SmartHome.Domain.DictionaryEntity;
using SmartHome.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.API.Persistence
{
    public static class AppInitialLoad
    {
        public static async Task Seed(IServiceProvider provider)
        {
            using (IServiceScope scope = provider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
                    .CreateLogger(nameof(AppInitialLoad));
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (!context.ControlStrategies.Any())
                {
                    var controlStrategies = new List<ControlStrategy>
                    {
                        new ControlStrategy
                        {
                            Id = 1,
                            IsActive = true,
                            Description = "Control over HTTP and REST",
                            Strategy = typeof(RestControlStrategy).FullName,
                            Key = "rest"
                        },
                        new ControlStrategy
                        {
                            Id = 2,
                            IsActive = true,
                            Description = "Control over MQTT",
                            Strategy = typeof(MqttControlStrategy).FullName,
                            Key = "mqtt"
                        }
                    };

                    await context.AddRangeAsync(controlStrategies);
                    await context.SaveChangesAsync();
                }

                if (!context.Dictionaries.Any(x => x.Name == "gender"))
                {
                    var dictionaries = new List<Dictionary>
                    {
                        new Dictionary
                        {
                            Id = 1,
                            Name = "gender",
                            Values = new List<DictionaryValue>
                            {
                                new DictionaryValue
                                {
                                    Id = 1,
                                    Value = "Male",
                                    IsActive = true,
                                },
                                new DictionaryValue
                                {
                                    Id = 2,
                                    Value = "Female",
                                    IsActive = true
                                }
                            }
                        }
                    };

                    await context.Dictionaries.AddRangeAsync(dictionaries);
                    await context.SaveChangesAsync();
                }

                if (!context.Dictionaries.Any(x => x.Name == "nodeType"))
                {
                    var dictionaries = new List<Dictionary>
                    {
                        new Dictionary
                        {
                            Id = 100,
                            Name = "NodeType",
                            Values = new List<DictionaryValue>
                            {
                                new DictionaryValue
                                {
                                    Id = 100,
                                    Value = "Sensor",
                                    IsActive = true,
                                },
                                new DictionaryValue
                                {
                                    Id = 101,
                                    Value = "Controllable device",
                                    IsActive = true
                                }
                            }
                        }
                    };

                    await context.Dictionaries.AddRangeAsync(dictionaries);
                    await context.SaveChangesAsync();
                }

                if (!context.Dictionaries.Any(x => x.Name == "sensorType"))
                {
                    var dictionaries = new List<Dictionary>
                    {
                        new Dictionary
                        {
                            Id = 200,
                            Name = "sensorType",
                            Values = new List<DictionaryValue>
                            {
                                new DictionaryValue
                                {
                                    Id = 200,
                                    Value = "Temperature",
                                    IsActive = true,
                                },
                                new DictionaryValue
                                {
                                    Id = 201,
                                    Value = "Sunlight",
                                    IsActive = true
                                },
                                new DictionaryValue
                                {
                                    Id = 202,
                                    Value = "Current",
                                    IsActive = true
                                },
                                new DictionaryValue
                                {
                                    Id = 203,
                                    Value = "Other",
                                    IsActive = true
                                }
                            }
                        }
                    };

                    await context.Dictionaries.AddRangeAsync(dictionaries);
                    await context.SaveChangesAsync();
                }

                if (!context.Nodes.Any(x => x.Name == "Dev"))
                {
                    var node = new Node()
                    {
                        Name = "Dev",
                        ControlStrategyId = 1,
                        Created = DateTime.UtcNow,
                        CreatedById = 1,
                        IpAddress = "http://192.168.0.101",
                        Port = 80,
                        GatewayIpAddress = "http://192.168.0.1",
                        Description = "Dev test node",
                    };

                    var createdNode = await context.Nodes.AddAsync(node);
                    await context.SaveChangesAsync();

                    context.Add(new AppUserNodeLink()
                    {
                        NodeId = createdNode.Entity.Id,
                        UserId = 1
                    });

                    await context.SaveChangesAsync();
                }

                if (!context.Commands.Any())
                {
                    var commands = new Collection<NodeCommand>
                    {
                        new NodeCommand()
                        {
                            Id = 1,
                            Name = "Relay0On",
                            BaseUri = "/api/relay/0",
                            Value = "1",
                            Type = "SET",
                            Description = "Espurna relay controller"
                        },
                        new NodeCommand()
                        {
                            Id = 2,
                            Name = "Relay0Off",
                            BaseUri = "/api/relay/0",
                            Value = "0",
                            Type = "SET",
                            Description = "Espurna relay controller"
                        },
                        new NodeCommand()
                        {
                            Id = 3,
                            Name = "Relay0Toggle",
                            BaseUri = "/api/relay/0",
                            Value = "2",
                            Type = "SET",
                            Description = "Espurna relay controller"
                        },
                        new NodeCommand()
                        {
                            Id = 4,
                            Name = "Relay1On",
                            BaseUri = "/api/relay/1",
                            Value = "1",
                            Type = "SET",
                            Description = "Espurna relay controller"
                        },
                        new NodeCommand()
                        {
                            Id = 5,
                            Name = "Relay1Off",
                            BaseUri = "/api/relay/1",
                            Value = "0",
                            Type = "SET",
                            Description = "Espurna relay controller"
                        },
                        new NodeCommand()
                        {
                            Id = 6,
                            Name = "Relay1Toggle",
                            BaseUri = "/api/relay/1",
                            Value = "2",
                            Type = "SET",
                            Description = "Espurna relay controller"
                        }
                    };

                    await context.Commands.AddRangeAsync(commands);
                    await context.SaveChangesAsync();

                    var links = new Collection<NodeCommandNodeLink>
                    {
                        new NodeCommandNodeLink
                        {
                            NodeId = 1,
                            NodeCommandId = 1
                        },
                        new NodeCommandNodeLink
                        {
                            NodeId = 1,
                            NodeCommandId = 2
                        },
                        new NodeCommandNodeLink
                        {
                            NodeId = 1,
                            NodeCommandId = 3
                        },
                        new NodeCommandNodeLink
                        {
                            NodeId = 1,
                            NodeCommandId = 4
                        }
                    };

                    foreach (var link in links)
                    {
                        context.Add(link);
                    }
                    await context.SaveChangesAsync();

                }
            }
        }
    }
}
