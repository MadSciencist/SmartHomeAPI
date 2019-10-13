using Microsoft.Extensions.DependencyInjection;
using SmartHome.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Entities.SchedulingEntity;

namespace SmartHome.Core.DataAccess.InitialLoad
{
    public class ConfigInitialLoad
    {
        public static async Task Seed(IServiceProvider provider)
        {
            using (IServiceScope scope = provider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (!context.RequestReasons.Any())
                {
                    var requestReasons = new List<DataRequestReason>
                    {
                        new DataRequestReason
                        {
                            Id = (int) EDataRequestReason.Node,
                            Reason = "Node",
                            Description = "Node was initiator"
                        },
                        new DataRequestReason
                        {
                            Id = (int) EDataRequestReason.Scheduler,
                            Reason = "Scheduler",
                            Description = "Task scheduler was initiator"
                        },
                        new DataRequestReason
                        {
                            Id = (int) EDataRequestReason.User,
                            Reason = "User",
                            Description = "User was initiator"
                        }
                    };

                    await context.AddRangeAsync(requestReasons);
                    await context.SaveChangesAsync();
                }

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
