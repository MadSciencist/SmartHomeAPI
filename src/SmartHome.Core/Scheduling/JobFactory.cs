using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;

namespace SmartHome.Core.Scheduling
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobType = bundle.JobDetail.JobType;
            return _serviceProvider.GetRequiredService(jobType) as IJob;
        }

        public void ReturnJob(IJob job) { }
    }
}
