using System;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Quartz;
using Quartz.Spi;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Entities.SchedulingEntity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
                    _logger.LogInformation($"Adding scheduled job: {job.ToString()}");

                    var jobSchedule = CreateJobSchedule(job);
                    await _scheduler.ScheduleJob(jobSchedule.CreateJob(), jobSchedule.CreateTrigger(),
                        cancellationToken);
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

        private JobSchedule CreateJobSchedule(ScheduleEntity job)
        {
            // TODO Think of more polymorphic way...
            var jobParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(job.JobParams);
            if (jobParams.ContainsKey(nameof(NodeJobSchedule.NodeId)) && jobParams.ContainsKey(nameof(NodeJobSchedule.Command)))
            {
                var nodeId = (int)(long)jobParams[nameof(NodeJobSchedule.NodeId)];
                var command = jobParams[nameof(NodeJobSchedule.Command)] as string;
                var commandParams = jobParams[nameof(NodeJobSchedule.CommandParams)];

                return new NodeJobSchedule(job.JobType.GetJobType(), nodeId, command, commandParams, job.CronExpression);
            }

            return new JobSchedule(job.JobType.GetJobType(), job.CronExpression);
        }
    }
}
