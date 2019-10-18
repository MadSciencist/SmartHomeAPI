using Microsoft.Extensions.DependencyInjection;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Entities.Enums;
using SmartHome.Core.Entities.SchedulingEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SmartHome.Core.Data.InitialLoad
{
    public class ConfigInitialLoad
    {
        public static async Task Seed(IServiceProvider provider)
        {
            using (IServiceScope scope = provider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<EntityFrameworkContext>();

                if (!context.JobTypes.Any())
                {
                    var jobTypes = new List<JobType>
                    {
                        new JobType
                        {
                            Id = 1000,
                            DisplayName = "Execute command on node",
                            FullyQualifiedName = "SmartHome.Core.Scheduling.Jobs.ExecuteNodeCommandJob",
                            AssemblyName = "SmartHome.Core"
                        }
                    };

                    await context.AddRangeAsync(jobTypes);
                    await context.SaveChangesAsync();
                }

                if (!context.ScheduleTypes.Any())
                {
                    var scheduleTypes = new List<ScheduleType>
                    {
                        new ScheduleType
                        {
                            Id = 2000,
                            DisplayName = "Generic schedule type",
                            FullyQualifiedName = "SmartHome.Core.Scheduling.JobSchedule",
                            AssemblyName = "SmartHome.Core"
                        },
                        new ScheduleType
                        {
                            Id = 2001,
                            DisplayName = "Node-centric schedule type",
                            FullyQualifiedName = "SmartHome.Core.Scheduling.NodeJobSchedule",
                            AssemblyName = "SmartHome.Core"
                        }
                    };

                    await context.AddRangeAsync(scheduleTypes);
                    await context.SaveChangesAsync();
                }

                if (!context.JobStatusEntity.Any())
                {
                    var entities = Enum.GetValues(typeof(JobStatus))
                        .Cast<int>()
                        .Where(i => !i.Equals(0))
                        .Select(e => new JobStatusEntity
                        {
                            Id = e,
                            Name = GetEnumMemberAttrValue<JobStatus>(Enum.Parse<JobStatus>(e.ToString()))
                        })
                        .ToList();

                    await context.AddRangeAsync(entities);
                    await context.SaveChangesAsync();
                }

                //        new PhysicalProperty("Light", "light", nameof(LightParam)),


                //        new PhysicalProperty("energy", "kWh"),
                //        new PhysicalProperty("energy_delta", "kWh"),
                //        new PhysicalProperty("Generic Analog Sensor", "generic_analog", "bit"),
                //        new PhysicalProperty("Generic digital (binary) Sensor", "generic_digital", "binary"),
                //        new PhysicalProperty("generic_event", ""),
                //        new PhysicalProperty("pm1dot0", "ppm"),
                //        new PhysicalProperty("pm1dot5", "ppm"),
                //        new PhysicalProperty("pm10", "ppm"),
                //        new PhysicalProperty("co2", "ppm"),
                //        new PhysicalProperty("lux", "lux"),
                //        new PhysicalProperty("distance", "m"),
                //        new PhysicalProperty("ldr_cpm", "events"),
                //        new PhysicalProperty("ldr_uSvh", "mSv"),
                //        new PhysicalProperty("count", "events"),
                if (!context.PhysicalProperties.Any())
                {
                    var properties = new List<PhysicalProperty>
                    {
                        new PhysicalProperty
                        {
                            Id = 100,
                            Name = "Temperature",
                            Magnitude = "temperature",
                            Unit = "C",
                            IsComplex = false
                        },
                        new PhysicalProperty
                        {
                            Id = 101,
                            Name = "Humidity",
                            Magnitude = "humidity",
                            Unit = "%",
                            IsComplex = false
                        },
                        new PhysicalProperty
                        {
                            Id = 102,
                            Name = "Pressure",
                            Magnitude = "pressure",
                            Unit = "hPa",
                            IsComplex = false
                        },
                        new PhysicalProperty
                        {
                            Id = 103,
                            Name = "Current",
                            Magnitude = "current",
                            Unit = "A",
                            IsComplex = false
                        },
                        new PhysicalProperty
                        {
                            Id = 104,
                            Name = "Voltage",
                            Magnitude = "voltage",
                            Unit = "V",
                            IsComplex = false
                        },
                        new PhysicalProperty
                        {
                            Id = 105,
                            Name = "Active power",
                            Magnitude = "power_active",
                            Unit = "W",
                            IsComplex = false
                        },
                        new PhysicalProperty
                        {
                            Id = 106,
                            Name = "Apparent power",
                            Magnitude = "power_apparent",
                            Unit = "VA",
                            IsComplex = false
                        },
                        new PhysicalProperty
                        {
                            Id = 107,
                            Name = "Reactive power",
                            Magnitude = "power_reactive",
                            Unit = "var",
                            IsComplex = false
                        },
                        new PhysicalProperty
                        {
                            Id = 108,
                            Name = "Power factor",
                            Magnitude = "power_factor",
                            Unit = "%",
                            IsComplex = false
                        },
                        new PhysicalProperty
                        {
                            Id = 109,
                            Name = "Relay #1",
                            Magnitude = "relay0",
                            Unit = "",
                            IsComplex = false
                        },
                        new PhysicalProperty
                        {
                            Id = 110,
                            Name = "Relay #2",
                            Magnitude = "relay1",
                            Unit = "",
                            IsComplex = false
                        },
                        new PhysicalProperty
                        {
                            Id = 111,
                            Name = "Relay #3",
                            Magnitude = "relay2",
                            Unit = "",
                            IsComplex = false
                        },
                        new PhysicalProperty
                        {
                            Id = 112,
                            Name = "Relay #4",
                            Magnitude = "relay3",
                            Unit = "",
                            IsComplex = false
                        }
                    };

                    await context.AddRangeAsync(properties);
                    await context.SaveChangesAsync();
                }
            }
        }

        // TODO move this somewhere
        public static string GetEnumMemberAttrValue<T>(T enumVal)
        {
            var enumType = typeof(T);
            var memInfo = enumType.GetMember(enumVal.ToString());
            var attr = memInfo.FirstOrDefault()?.GetCustomAttributes(false).OfType<EnumMemberAttribute>().FirstOrDefault();
            return attr?.Value;
        }
    }
}
