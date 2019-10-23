using Autofac;
using Matty.Framework;
using Matty.Framework.Abstractions;
using Matty.Framework.Enums;
using Microsoft.Extensions.Configuration;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Infrastructure.Exceptions;
using SmartHome.Core.Repositories;
using SmartHome.Core.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class NodeDataService : CrudServiceBase<object, EntityBase>, INodeDataService
    {
        private readonly INodeRepository _nodeRepository;
        private readonly INodeDataRepository _nodeDataRepository;
        private readonly IAuthorizationProvider<Node> _authorizationProvider;
        private readonly IPhysicalPropertyService _physicalPropertyService;

        public NodeDataService(ILifetimeScope container,
            INodeRepository nodeRepository,
            INodeDataRepository nodeDataRepository,
            IAuthorizationProvider<Node> authorizationProvider,
            IPhysicalPropertyService physicalPropertyService) : base(container)
        {
            _nodeRepository = nodeRepository;
            _nodeDataRepository = nodeDataRepository;
            _authorizationProvider = authorizationProvider;
            _physicalPropertyService = physicalPropertyService;
        }

        public async Task<NodeData> AddSingleAsync(int nodeId, NodeDataDto data)
        {
            var samplesToKeep = Config.GetValue<int>("Defaults:NodeDataRetention:SamplesToKeep");

            var nodeData = new NodeData
            {
                PhysicalProperty = data.PhysicalProperty,
                PhysicalPropertyId = data.PhysicalProperty.Id,
                Value = data.Value,
                NodeId = nodeId,
                TimeStamp = DateTime.UtcNow
            };

            return await _nodeDataRepository.AddSingleAsync(nodeData, samplesToKeep);
        }

        public async Task AddManyAsync(int nodeId, IEnumerable<NodeDataDto> data)
        {
            var samplesToKeep = Config.GetValue<int>("Defaults:NodeDataRetention:SamplesToKeep");

            var nodeData = data.Select(x => new NodeData
            {
                PhysicalProperty = x.PhysicalProperty,
                PhysicalPropertyId = x.PhysicalProperty.Id,
                Value = x.Value,
                NodeId = nodeId,
                TimeStamp = DateTime.UtcNow
            });

            await _nodeDataRepository.AddManyAsync(nodeId, samplesToKeep, nodeData);
        }

        public async Task<ServiceResult<NodeDataResultDto>> GetNodeDataByMagnitude(int nodeId, string magnitude, int limit)
        {
            var node = await _nodeRepository.GetByIdAsync(nodeId);
            if (node is null) throw new SmartHomeEntityNotFoundException($"Node with id: {nodeId} does not exists.");

            var physicalProperty = await _physicalPropertyService.GetByMagnitudeAsync(magnitude);
            if (physicalProperty is null) throw new SmartHomeEntityNotFoundException($"Physical property with id: {magnitude} does not exists.");

            if (!_authorizationProvider.Authorize(null, Principal, OperationType.Read))
                throw new SmartHomeUnauthorizedException(
                    $"User ${Principal.Identity.Name} is not authorized to add new node");

            var results = await _nodeDataRepository.GetByMagnitudeAsync(nodeId, magnitude, limit);

            return new ServiceResult<NodeDataResultDto>(Principal)
            {
                Data = new NodeDataResultDto
                {
                    Values = results.Select(x => new NodeDataRecordDto(x.TimeStamp, x.Value)),
                    PhysicalProperty = physicalProperty.Data
                }
            };
        }
    }
}
