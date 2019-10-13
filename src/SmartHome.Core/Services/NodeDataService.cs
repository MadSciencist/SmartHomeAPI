using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Dto;
using SmartHome.Core.Dto.NodeData;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Entities.Enums;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Infrastructure.Exceptions;
using SmartHome.Core.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Matty.Framework;
using Matty.Framework.Enums;

namespace SmartHome.Core.Services
{
    public class NodeDataService : CrudServiceBase<object, EntityBase>, INodeDataService
    {
        private readonly INodeDataRepository _nodeDataRepository;
        private readonly INodeDataMagnitudeRepository _nodeDataMagnitudeRepository;

        public NodeDataService(ILifetimeScope container, INodeDataRepository nodeDataRepository, INodeDataMagnitudeRepository nodeDataMagnitudeRepository) : base(container)
        {
            _nodeDataRepository = nodeDataRepository;
            _nodeDataMagnitudeRepository = nodeDataMagnitudeRepository;
        }

        public async Task<ServiceResult<ICollection<NodeMagnitudeData>>> GetNodeDatas(int nodeId, int pageNumber, int pageSize,
            string[] properties, DateTime from, DateTime to, DataOrder order, int maxCount = 1000, bool paged = false)
        {
            if (properties.Length == 0 || properties is null) throw new SmartHomeException("No properties selected.");

            var response = new ServiceResult<ICollection<NodeMagnitudeData>>(Principal);

            var tasks = properties.Select(async p => await GetNodeMagnitudeData(nodeId, pageNumber, pageSize, p, from, to, order, maxCount, paged)).ToArray();

            await Task.WhenAll(tasks);

            response.Data = tasks.Select(t => t.Status == TaskStatus.RanToCompletion ? t.Result : throw new Exception("Task Not Completed Successfully")).ToList();

            return response;
        }

        private async Task<NodeMagnitudeData> GetNodeMagnitudeData(int nodeId, int pageNumber, int pageSize,
            string property, DateTime from, DateTime to, DataOrder order, int maxCount, bool paged = false)
        {
            var query = _nodeDataMagnitudeRepository.AsQueryableNoTrack()
                .Where(x => x.NodeData.NodeId == nodeId)
                .Where(x => x.NodeData.TimeStamp >= from && x.NodeData.TimeStamp <= to)
                .Where(x => x.Magnitude == property)
                .Select(x => new NodeMagnitudeRecord { TimeStamp = x.NodeData.TimeStamp, Value = x.Value });

            query = order == DataOrder.Asc
                        ? query.OrderBy(p => p.TimeStamp)
                        : query.OrderByDescending(p => p.TimeStamp);

            if(!paged)
                query = query.Take(maxCount);

            Logger.LogTrace($"Generated SQL: {query.ToSql()}");

            if (paged)
            {
                return new NodeMagnitudeDataPaged
                {
                    Magnitude = property,
                    PagedData = await query.GetPagedAsync(pageNumber, pageSize)
                };
            }

            return new NodeMagnitudeData
            {
                Magnitude = property,
                Data = await query.ToListAsync()
            };
        }

        public async Task<NodeData> AddSingleAsync(int nodeId, NodeDataMagnitudeDto data)
        {
            var samplesToKeep = Config.GetValue<int>("Defaults:NodeDataRetention:SamplesToKeep");

            return await _nodeDataRepository.AddSingleAsync(nodeId, samplesToKeep, new NodeDataMagnitude
            {
                Magnitude = data.PhysicalProperty.Magnitude,
                Unit = data.PhysicalProperty.Unit,
                Value = data.Value
            });
        }

        public async Task<NodeData> AddManyAsync(int nodeId, IEnumerable<NodeDataMagnitudeDto> data)
        {
            var samplesToKeep = Config.GetValue<int>("Defaults:NodeDataRetention:SamplesToKeep");

            return await _nodeDataRepository.AddManyAsync(nodeId, samplesToKeep, data.Select(x =>
                new NodeDataMagnitude
                {
                    Magnitude = x.PhysicalProperty.Magnitude,
                    Unit = x.PhysicalProperty.Unit,
                    Value = x.Value
                }).ToList());
        }
    }
}
