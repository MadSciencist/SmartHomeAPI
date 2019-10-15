using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using Quartz.Spi;
using SmartHome.Core.Data.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.Core.Scheduling
{
    public class SchedulerHostedService : IHostedService
    {
        private IScheduler _scheduler;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly ISchedulesPersistenceRepository _scheduleRepository;
        private readonly ILogger<SchedulerHostedService> _logger;

        public SchedulerHostedService(ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory,
            ISchedulesPersistenceRepository scheduleRepo,
            ILogger<SchedulerHostedService> logger)
        {
            _schedulerFactory = schedulerFactory;
            _scheduleRepository = scheduleRepo;
            _logger = logger;
            _jobFactory = jobFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting hosted service: {nameof(SchedulerHostedService)}");
            try
            {
                _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
                _scheduler.JobFactory = _jobFactory;

                var persistedJobs = await _scheduleRepository.GetAllAsync();

                foreach (var job in persistedJobs)
                {
                    if (job.JobStatusEntity.AsEnum() == Entities.Enums.JobStatus.Running)
                    {
                        _logger.LogInformation($"Adding scheduled job: {job}");

                        var jobSchedule = JsonConvert.DeserializeObject(job.SerializedJobSchedule, job.ScheduleType.GetScheduleType()) as JobSchedule;

                        await _scheduler.ScheduleJob(jobSchedule.CreateJob(), jobSchedule.CreateTrigger(),
                            cancellationToken);
                    }
                }

                await _scheduler.Start(cancellationToken);
                _logger.LogInformation("Successfully started scheduling service.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error while starting scheduler");
                throw;
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Shutting down scheduling service.");
            if (_scheduler != null) await _scheduler.Shutdown(cancellationToken);
        }
    }
}
