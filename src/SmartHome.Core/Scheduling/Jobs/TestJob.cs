using Autofac;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Threading.Tasks;

namespace SmartHome.Core.Scheduling.Jobs
{
    [DisallowConcurrentExecution]
    public class TestJob : JobBase
    {
        public TestJob(ILifetimeScope container) : base(container)
        {
        }
        
        public override Task Execute(IJobExecutionContext context)
        {
            Logger.LogDebug($"TEST JOB TEST JOB TEST JOB TEST JOB TEST JOB TEST JOB");

            return Task.CompletedTask;
        }
    }
}
