using Microsoft.Extensions.DependencyInjection;
using SmartHome.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                    var requestReasons = new List<Domain.Entity.DataRequestReason>
                    {
                        new Domain.Entity.DataRequestReason
                        {
                            Id = (int) EDataRequestReason.Node,
                            Reason = "Node",
                            Description = "Node was initiator"
                        },
                        new Domain.Entity.DataRequestReason
                        {
                            Id = (int) EDataRequestReason.Scheduler,
                            Reason = "Scheduler",
                            Description = "Task scheduler was initiator"
                        },
                        new Domain.Entity.DataRequestReason
                        {
                            Id = (int) EDataRequestReason.User,
                            Reason = "User",
                            Description = "User was initiator"
                        }
                    };

                    await context.AddRangeAsync(requestReasons);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
