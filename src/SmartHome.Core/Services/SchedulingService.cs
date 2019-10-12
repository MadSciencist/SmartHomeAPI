using Autofac;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Quartz;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Entities.Enums;
using SmartHome.Core.Entities.SchedulingEntity;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Infrastructure.Exceptions;
using SmartHome.Core.Infrastructure.Validators;
using SmartHome.Core.Scheduling;
using SmartHome.Core.Security;
using SmartHome.Core.Services.Abstractions;
using SmartHome.Core.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class SchedulingService : ServiceBase, ISchedulingService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IGenericRepository<JobType> _jobTypeRepository;
        private readonly IValidator _validator;
        private readonly NodeAuthorizationProvider _authorizationProvider;
        private readonly INodeRepository _nodeRepository;
        private readonly ISchedulesPersistenceRepository _scheduleRepository;

        public SchedulingService(ILifetimeScope container, 
            ISchedulerFactory schedulerFactory, 
            INodeRepository nodeRepository,
            ISchedulesPersistenceRepository scheduleRepository,
            IGenericRepository<JobType> jobTypeRepository,
            IValidator validator,
            NodeAuthorizationProvider authorizationProvider) : base(container)
        {
            _schedulerFactory = schedulerFactory;
            _nodeRepository = nodeRepository;
            _scheduleRepository = scheduleRepository;
            _jobTypeRepository = jobTypeRepository;
            _validator = validator;
            _authorizationProvider = authorizationProvider;
        }

        public async Task<ServiceResult<SchedulesPersistence>> AddNodeCommandJobAsync(ScheduleNodeCommandJobDto param)
        {
            var response = new ServiceResult<SchedulesPersistence>(Principal);
            var validationResult = _validator.Validate(param);

            if (!_validator.Validate(param).IsValid)
            {
                response.Alerts = validationResult.GetValidationMessages();
                return response;
            }

            if (!_authorizationProvider.Authorize(null, Principal, OperationType.Execute))
            {
                throw new SmartHomeUnauthorizedException(
                    $"User ${Principal.Identity.Name} is not authorized to use this node");
            }

            var scheduler = await _schedulerFactory.GetScheduler(CancellationToken.None);
            var (node, executorType) = await GetNodeAndJobTypeEntities(param);

            var jobSchedule = new NodeJobSchedule(executorType, node.Id, param.Command, param.CommandParams, param.CronExpression);
            
            using (var transaction = _scheduleRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    await scheduler.ScheduleJob(jobSchedule.CreateJob(), jobSchedule.CreateTrigger());

                    var entity = new SchedulesPersistence
                    {
                        Name = param.Name,
                        CronExpression = param.CronExpression,
                        JobTypeId = param.JobTypeId,
                        CreatedById = GetCurrentUserId(),
                        Created = DateTime.UtcNow,
                        JobParams = new SerializableParamBuilder()
                            .Add(nameof(param.NodeId), param.NodeId)
                            .Add(nameof(param.Command), param.Command)
                            .Add(nameof(param.CommandParams), param.CommandParams) // TODO validate params against schema
                            .Build()
                    };

                    var created = await _scheduleRepository.CreateAsync(entity);
                    transaction.Commit();
                    response.Alerts.Add(new Alert("Successfully added new job.", MessageType.Success));
                    response.Data = created;
                }
                catch (ObjectAlreadyExistsException)
                {
                    response.Alerts.Add(new Alert("Job with given parameters already exists.", MessageType.Error));
                    return response;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Rolling back transaction {transaction.TransactionId}");
                    transaction.Rollback();
                    throw;
                }
            }

            return response;
        }

        private async Task<Tuple<Node, Type>> GetNodeAndJobTypeEntities(ScheduleNodeCommandJobDto param)
        {
            var node = await _nodeRepository.GetByIdAsync(param.NodeId);
            var jobType = await _jobTypeRepository.GetByIdAsync(param.JobTypeId);

            if (node is null || jobType is null)
                throw new SmartHomeEntityNotFoundException($"Either given nodeId or jobTypeId doesn't exists");

            var executorType = jobType.GetJobType();

            return new Tuple<Node, Type>(node, executorType);
        }

        public Task<ServiceResult<IEnumerable<SchedulesPersistence>>> GetJobs()
        {
            // TODO 
            var response = new ServiceResult<IEnumerable<SchedulesPersistence>>(Principal);
            var jobs = _scheduleRepository.GetAll();

            response.Data = jobs;
            return Task.FromResult(response);
        }
    }
}
