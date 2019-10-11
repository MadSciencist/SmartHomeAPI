using Autofac;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Threading.Tasks;

namespace SmartHome.Core.Scheduling.Jobs
{
    [DisallowConcurrentExecution]
    public class ExecuteNodeCommandJob : JobBase
    {
        public ExecuteNodeCommandJob(ILifetimeScope container) : base(container)
        {
        }
        
        public override Task Execute(IJobExecutionContext context)
        {
            Logger.LogDebug($"NODE EXECUTION NODE EXECUTION NODE EXECUTION NODE EXECUTION");

            return Task.CompletedTask;
        }
    }
}
