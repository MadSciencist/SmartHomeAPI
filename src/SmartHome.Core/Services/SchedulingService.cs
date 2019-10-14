using Autofac;
using AutoMapper;
using FluentValidation;
using Matty.Framework;
using Matty.Framework.Abstractions;
using Matty.Framework.Enums;
using Microsoft.Extensions.Logging;
using Quartz;
using SmartHome.Core.Data.Repository;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Entities.Enums;
using SmartHome.Core.Entities.SchedulingEntity;
using SmartHome.Core.Infrastructure.Exceptions;
using SmartHome.Core.Infrastructure.Validators;
using SmartHome.Core.Scheduling;
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
        private readonly IMapper _mapper;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IGenericRepository<JobType> _jobTypeRepository;
        private readonly IValidator<ScheduleNodeCommandJobDto> _validator;
        private readonly IAuthorizationProvider<Node> _nodeAuth;
        private readonly IAuthorizationProvider<ScheduleEntity> _schedulesAuth;
        private readonly INodeRepository _nodeRepository;
        private readonly ISchedulesPersistenceRepository _scheduleRepository;

        public SchedulingService(ILifetimeScope container, 
            IMapper mapper,
            ISchedulerFactory schedulerFactory, 
            INodeRepository nodeRepository,
            ISchedulesPersistenceRepository scheduleRepository,
            IGenericRepository<JobType> jobTypeRepository,
            IValidator<ScheduleNodeCommandJobDto> validator,
            IAuthorizationProvider<Node> nodeAuth,
            IAuthorizationProvider<ScheduleEntity> schedulesAuth) : base(container)
        {
            _mapper = mapper;
            _schedulerFactory = schedulerFactory;
            _nodeRepository = nodeRepository;
            _scheduleRepository = scheduleRepository;
            _jobTypeRepository = jobTypeRepository;
            _validator = validator;
            _nodeAuth = nodeAuth;
            _schedulesAuth = schedulesAuth;
        }

        public async Task<ServiceResult<JobScheduleDto>> AddNodeCommandJobAsync(ScheduleNodeCommandJobDto param)
        {
            var response = new ServiceResult<JobScheduleDto>(Principal);
            var validationResult = _validator.Validate(param);

            if (!_validator.Validate(param).IsValid)
            {
                response.AddAlerts(validationResult.GetValidationMessages());
                return response;
            }

            if (!_nodeAuth.Authorize(null, Principal, OperationType.Execute))
            {
                throw new SmartHomeUnauthorizedException(
                    $"User ${Principal.Identity.Name} is not authorized to use this node");
            }

            var scheduler = await _schedulerFactory.GetScheduler(CancellationToken.None);
            var (node, executorType) = await GetNodeAndJobTypeEntities(param);

            var jobSchedule = new NodeJobSchedule(executorType, node.Id, param.Command, param.CommandParams, param.CronExpression);
            
            using (var transaction = _scheduleRepository.BeginTransaction())
            {
                try
                {
                    await scheduler.ScheduleJob(jobSchedule.CreateJob(), jobSchedule.CreateTrigger());

                    var entity = new ScheduleEntity
                    {
                        Name = param.Name,
                        JobName = jobSchedule.GetIdentity(),
                        JobGroup = "DEFAULT",
                        JobStatusEntityId = (int)JobStatus.Running,
                        CronExpression = param.CronExpression,
                        JobTypeId = param.JobTypeId,
                        JobParams = new SerializableParamBuilder()
                            .Add(nameof(param.NodeId), param.NodeId)
                            .Add(nameof(param.Command), param.Command)
                            .Add(nameof(param.CommandParams), param.CommandParams) // TODO validate params against schema
                            .Build()
                    };

                    var created = await _scheduleRepository.CreateAsync(entity);
                    transaction.Commit();
                    response.Alerts.Add(new Alert("Successfully added new job.", MessageType.Success));
                    response.Data = _mapper.Map<JobScheduleDto>(created);
                }
                catch (ObjectAlreadyExistsException)
                {
                    response.Alerts.Add(new Alert("Job with given parameters already exists.", MessageType.Error));
                    return response;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Rolling back transaction {transaction.Identifier}");
                    transaction.Rollback();
                    await scheduler.DeleteJob(new JobKey(jobSchedule.GetIdentity(), "DEFAULT"));
                    throw;
                }
            }

            return response;
        }

        public async Task<ServiceResult<JobScheduleDto>> UpdateJobStatus(int id, JobStatus status)
        {
            var response = new ServiceResult<JobScheduleDto>(Principal);
            var scheduler = await _schedulerFactory.GetScheduler(CancellationToken.None);
            var schedule = await _scheduleRepository.GetByIdAsync(id);

            if (schedule is null)
                throw new SmartHomeEntityNotFoundException($"Schedule with id: {id} does not exists.");

            if (!_schedulesAuth.Authorize(schedule, Principal, OperationType.Modify))
                throw new SmartHomeUnauthorizedException($"User ${Principal.Identity.Name} is not authorized to edit this schedule");

            using (var transaction = _scheduleRepository.BeginTransaction())
            {
                try
                {
                    var jobKey = new JobKey(schedule.JobName, schedule.JobGroup);
                    if (status == JobStatus.Running)
                        await scheduler.ResumeJob(jobKey);
                    else if (status == JobStatus.Paused)
                        await scheduler.PauseJob(jobKey);
                    else throw new InvalidOperationException();

                    schedule.JobStatusEntityId = (int)status;
                    var updated = await _scheduleRepository.UpdateAsync(schedule);
                    transaction.Commit();
                    response.Data = _mapper.Map<JobScheduleDto>(updated);
                    response.AddSuccessMessage($"Successfully updated job {schedule.Name} status to {status.ToString()}");
                    return response;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Rolling back transaction {transaction.Identifier}");
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<ServiceResult<object>> RemoveJob(int id)
        {
            var response = new ServiceResult<object>(Principal);
            var scheduler = await _schedulerFactory.GetScheduler(CancellationToken.None);
            var schedule = await _scheduleRepository.GetByIdAsync(id);

            if (schedule is null)
                throw new SmartHomeEntityNotFoundException($"Schedule with id: {id} does not exists.");

            if (!_schedulesAuth.Authorize(schedule, Principal, OperationType.HardDelete))
                throw new SmartHomeUnauthorizedException($"User ${Principal.Identity.Name} is not authorized to delete this schedule");

            using (var transaction = _scheduleRepository.BeginTransaction())
            {
                try
                {
                    var jobKey = new JobKey(schedule.JobName, schedule.JobGroup);
                    await scheduler.DeleteJob(jobKey);

                    await _scheduleRepository.DeleteAsync(schedule);
                    transaction.Commit();
                    response.AddSuccessMessage($"Successfully deleted {schedule.Name} job.");
                    return response;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Rolling back transaction {transaction.Identifier}");
                    transaction.Rollback();
                    throw;
                }
            }
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

        public async Task<ServiceResult<IEnumerable<JobScheduleDto>>> GetJobs()
        {
            return new ServiceResult<IEnumerable<JobScheduleDto>>(Principal)
            {
                Data = _mapper.Map<IEnumerable<JobScheduleDto>>(await _scheduleRepository.GetAllAsync())
            };
        }

        public async Task<ServiceResult<JobScheduleDto>> GeJobById(int id)
        {
            return new ServiceResult<JobScheduleDto>(Principal)
            {
                Data = _mapper.Map<JobScheduleDto>(await _scheduleRepository.GetByIdAsync(id))
            };
        }
    }
}
