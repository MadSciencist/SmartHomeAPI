using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartHome.Domain.DictionaryEntity;
using SmartHome.Domain.Entity;

namespace SmartHome.Core.DataAccess.InitialLoad
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
                            Description = "Control ESPURNA device over HTTP and REST",
                            ExecutorClassNamespace = "SmartHome.Core.Providers.Rest.Contracts.Espurna",
                            Name = "rest",
                            Type = ControlStrategyType.Rest
                        },
                        new ControlStrategy
                        {
                            Id = 2,
                            IsActive = true,
                            Description = "Control over MQTT",
                            ExecutorClassNamespace = "SmartHome.Core.Providers.Mqtt.Contracts.Espurna",
                            Name = "mqtt",
                            Type = ControlStrategyType.Mqtt
                        }
                    };

                    await context.AddRangeAsync(controlStrategies);
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
                        ApiKey = "03102E55CD7BBE35"
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
                    var commands = new Collection<Command>
                    {
                        new Command()
                        {
                            Id = 1,
                            Name = "getAvailableSensors",
                            ExecutorClassName = ""
                        },
                        new Command()
                        {
                            Id = 100,
                            Name = "toggleRelay",
                            ExecutorClassName = "ToggleRelay" // class name, move namespace to controlStrategy
                        },
                         new Command()
                        {
                            Id = 101,
                            Name = "setRelay",
                            ExecutorClassName = "SetRelay" // class name, move namespace to controlStrategy
                        },
                    };

                    await context.Commands.AddRangeAsync(commands);
                    await context.SaveChangesAsync();

                    var links = new Collection<ControlStrategyCommandLink>
                    {
                        new ControlStrategyCommandLink
                        {
                            ControlStrategyId = 1,
                            CommandId = 1
                        },
                        new ControlStrategyCommandLink
                        {
                            ControlStrategyId = 1,
                            CommandId = 100
                        },
                        new ControlStrategyCommandLink
                        {
                            ControlStrategyId = 1,
                            CommandId = 101
                        }
                    };

                    foreach (var link in links)
                    {
                        context.Add(link);
                    }

                    await context.SaveChangesAsync();
                }


                // currently not used
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
            }
        }
    }
}