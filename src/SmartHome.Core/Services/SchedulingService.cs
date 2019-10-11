using Autofac;
using Quartz;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Entities.SchedulingEntity;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Infrastructure.Exceptions;
using SmartHome.Core.Scheduling;
using SmartHome.Core.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartHome.Core.Entities.Enums;
using SmartHome.Core.Security;
using FluentValidation;
using SmartHome.Core.Infrastructure.Validators;

namespace SmartHome.Core.Services
{
    public class SchedulingService : ServiceBase, ISchedulingService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IGenericRepository<JobType> _jobTypeRepository;
        private readonly NodeAuthorizationProvider _authorizationProvider;
        private readonly INodeRepository _nodeRepository;

        public SchedulingService(ILifetimeScope container, 
            ISchedulerFactory schedulerFactory, 
            INodeRepository nodeRepository, 
            IGenericRepository<JobType> jobTypeRepository,
            NodeAuthorizationProvider authorizationProvider) : base(container)
        {
            _schedulerFactory = schedulerFactory;
            _nodeRepository = nodeRepository;
            _jobTypeRepository = jobTypeRepository;
            _authorizationProvider = authorizationProvider;
        }

        public async Task<ServiceResult<DateTimeOffset>> AddRepeatableJobAsync(ExecuteNodeCommandJobDto param)
        {
            var response = new ServiceResult<DateTimeOffset>(Principal);
            var validationResult = Container.Resolve<IValidator<ExecuteNodeCommandJobDto>>().Validate(param);

            if (!validationResult.IsValid)
            {
                response.Alerts = validationResult.GetValidationMessages();
                return response;
            }

            if (!_authorizationProvider.Authorize(null, Principal, OperationType.Execute))
            {
                throw new SmartHomeUnauthorizedException(
                    $"User ${Principal.Identity.Name} is not authorized to add new node");
            }

            var scheduler = await _schedulerFactory.GetScheduler(CancellationToken.None);
            var (node, executorType) = await GetNodeAndJobTypeEntities(param);

            var jobSchedule = new NodeJobSchedule(executorType, node, param.CronExpression);

            var job = JobCreationHelper.CreateJob(jobSchedule);
            var trigger = JobCreationHelper.CreateTrigger(jobSchedule);

            response.Data = await scheduler.ScheduleJob(job, trigger);
            response.Alerts.Add(new Alert("Successfully added new job", MessageType.Success));
            return response;
        }

        private async Task<Tuple<Node, Type>> GetNodeAndJobTypeEntities(ExecuteNodeCommandJobDto param)
        {
            var node = await _nodeRepository.GetByIdAsync(param.NodeId);
            var jobType = await _jobTypeRepository.GetByIdAsync(param.JobTypeId);

            if (node is null || jobType is null)
                throw new SmartHomeEntityNotFoundException($"Either given nodeId or jobTypeId doesn't exists");

            var executorType = jobType.GetJobType();

            return new Tuple<Node, Type>(node, executorType);
        }

        // TODO: job persistence in DB
        public async Task<ServiceResult<List<string>>> GetJobs()
        {
            var response = new ServiceResult<List<string>>(Principal);
            var scheduler = await _schedulerFactory.GetScheduler(CancellationToken.None);
            IReadOnlyCollection<IJobExecutionContext> jobs = await scheduler.GetCurrentlyExecutingJobs();
            response.Data = jobs.Select(x => x.JobDetail.Key.Name).ToList();
            return response;
        }
    }
}
