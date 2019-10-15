using Microsoft.Extensions.DependencyInjection;
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
