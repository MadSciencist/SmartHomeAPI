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
                            ProviderName = "Mqtt",
                            ContextName = "Espurna",
                        },
                        new ControlStrategy
                        {
                            Id = 2,
                            IsActive = true,
                            Description = "Control over MQTT",
                            ProviderName = "Mqtt",
                            ContextName = "Espurna"
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
                        ControlStrategyId = 2,
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
                        new Command
                        {
                            Id = 1,
                            Alias = "getAvailableSensors",
                            ExecutorClassName = ""
                        },
                        new Command
                        {
                            Id = 100,
                            Alias = "toggleRelay",
                            ExecutorClassName = "ToggleRelay" // class name, move namespace to controlStrategy
                        },
                         new Command
                        {
                            Id = 101,
                            Alias = "setRelay",
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
                        },
                        new ControlStrategyCommandLink
                        {
                            ControlStrategyId = 2,
                            CommandId = 1
                        },
                        new ControlStrategyCommandLink
                        {
                            ControlStrategyId = 2,
                            CommandId = 100
                        },
                        new ControlStrategyCommandLink
                        {
                            ControlStrategyId = 2,
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

                if (!context.Dictionaries.Any(x => x.Name == "ControlProvider"))
                {
                    var dictionaries = new List<Dictionary>
                    {
                        new Dictionary
                        {
                            Id = 100,
                            Name = "ControlProvider",
                            Values = new List<DictionaryValue>
                            {
                                new DictionaryValue
                                {
                                    Id = 100,
                                    Value = "Mqtt",
                                    IsActive = true,
                                },
                                new DictionaryValue
                                {
                                    Id = 101,
                                    Value = "Mqtt",
                                    IsActive = true
                                }
                            }
                        }
                    };

                    await context.Dictionaries.AddRangeAsync(dictionaries);
                    await context.SaveChangesAsync();
                }

                if (!context.Dictionaries.Any(x => x.Name == "ControlContext"))
                {
                    var dictionaries = new List<Dictionary>
                    {
                        new Dictionary
                        {
                            Id = 200,
                            Name = "ControlContext",
                            Values = new List<DictionaryValue>
                            {
                                new DictionaryValue
                                {
                                    Id = 200,
                                    Value = "Espurna",
                                    IsActive = true,
                                },
                                new DictionaryValue
                                {
                                    Id = 201,
                                    Value = "Custom",
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