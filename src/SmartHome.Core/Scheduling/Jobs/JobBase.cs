using Quartz;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Logging;

namespace SmartHome.Core.Scheduling.Jobs
{
    public abstract class JobBase : IJob
    {
        public ILifetimeScope Container { get; }

        private ILogger _logger;
        protected ILogger Logger => _logger ?? (_logger = Container.Resolve<ILoggerFactory>().CreateLogger(this.GetType().FullName));

        protected JobBase(ILifetimeScope container)
        {
            Container = container;
        }

        public abstract Task Execute(IJobExecutionContext context);
    }
}
