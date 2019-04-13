using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartHome.Domain.Entity;
using SmartHome.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartHome.Domain.DictionaryEntity;

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



                if (!context.NodeTypes.Any())
                {
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

                    //logger.LogInformation("Truncating node_type table");
                    //await context.Database.ExecuteSqlCommandAsync("SET FOREIGN_KEY_CHECKS = 0;TRUNCATE TABLE node_type;SET FOREIGN_KEY_CHECKS = 1;");


                    logger.LogInformation("Loading node types into node_type table");
                    await context.AddRangeAsync(nodeTypes);
                    await context.SaveChangesAsync();
                }

                if (!context.ControlStrategies.Any())
                {
                    var controlStrategies = new List<ControlStrategy>
                    {
                        new ControlStrategy
                        {
                            Id = 1,
                            IsActive = true,
                            Description = "Control over HTTP and REST",
                            Strategy = "SmartHome.DeviceController.Rest.RestControlStrategy",
                            Key = "rest"
                        },
                        new ControlStrategy
                        {
                            Id = 2,
                            IsActive = true,
                            Description = "Control over MQTT",
                            Strategy = "SmartHome.DeviceController.Mqtt.MqttControlStrategy",
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
            }
        }
    }
}
