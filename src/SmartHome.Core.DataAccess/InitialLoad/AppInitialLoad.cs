using Microsoft.Extensions.DependencyInjection;
using SmartHome.Core.Domain.Entity;
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
                            ReceiveContext = "Espurna",
                            CreatedById = 1
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
                            RegisteredSensors = new List<RegisteredSensors>()
                            {
                                new RegisteredSensors
                                {
                                    Name = "analog",
                                    Description = "Generic built-in analog sensor"
                                }
                            },
                            CreatedById = 1
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
                        ApiKey = "03102E55CD7BBE35",
                        ClientId = "clientId"
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
            }
        }
    }
}