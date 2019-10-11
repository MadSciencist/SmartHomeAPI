using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.Core.Scheduling
{
    public class SchedulerHostedService : IHostedService
    {
        public IScheduler Scheduler { get; set; }

        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<JobSchedule> _jobSchedules;

        public SchedulerHostedService(ISchedulerFactory schedulerFactory, IJobFactory jobFactory, IEnumerable<JobSchedule> jobSchedules)
        {
            _schedulerFactory = schedulerFactory;
            _jobSchedules = jobSchedules;
            _jobFactory = jobFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;

            foreach (var jobSchedule in _jobSchedules)
            {
                var job = JobCreationHelper.CreateJob(jobSchedule);
                var trigger = JobCreationHelper.CreateTrigger(jobSchedule);

                await Scheduler.ScheduleJob(job, trigger, cancellationToken);
            }

            await Scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (Scheduler != null) await Scheduler.Shutdown(cancellationToken);
        }
    }
}
