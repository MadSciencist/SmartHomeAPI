using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartHome.Domain.Entity;
using SmartHome.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            }
        }
    }
}
