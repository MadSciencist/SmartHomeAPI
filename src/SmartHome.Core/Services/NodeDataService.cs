using Autofac;
using Matty.Framework;
using Matty.Framework.Enums;
using Microsoft.Extensions.Configuration;
using SmartHome.Core.Dto;
using SmartHome.Core.Dto.NodeData;
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
        private readonly INodeDataRepository _nodeDataRepository;

        public NodeDataService(ILifetimeScope container, INodeDataRepository nodeDataRepository) : base(container)
        {
            _nodeDataRepository = nodeDataRepository;
        }

        public async Task<ServiceResult<ICollection<NodeMagnitudeData>>> GetNodeDatas(int nodeId, int pageNumber, int pageSize,
            string[] properties, DateTime from, DateTime to, DataOrder order, int maxCount = 1000, bool paged = false)
        {
            if (properties.Length == 0 || properties is null) throw new SmartHomeException("No properties selected.");

            var response = new ServiceResult<ICollection<NodeMagnitudeData>>(Principal);

            return response;
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

        //public async Task<NodeData> AddManyAsync(int nodeId, IEnumerable<NodeDataMagnitudeDto> data)
        //{
        //    var samplesToKeep = Config.GetValue<int>("Defaults:NodeDataRetention:SamplesToKeep");

        //    return await _nodeDataRepository.AddManyAsync(nodeId, samplesToKeep, data.Select(x =>
        //        new NodeDataMagnitude
        //        {
        //            Magnitude = x.PhysicalProperty.Magnitude,
        //            Unit = x.PhysicalProperty.Unit,
        //            Value = x.Value
        //        }).ToList());
        //}
    }
}
