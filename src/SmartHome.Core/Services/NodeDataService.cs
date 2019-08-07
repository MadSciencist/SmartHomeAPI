using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Dto;
using SmartHome.Core.Dto.NodeData;
using SmartHome.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class NodeDataService : ServiceBase<object, object>, INodeDataService
    {
        private readonly INodeDataRepository _nodeDataRepository;

        public NodeDataService(ILifetimeScope container, INodeDataRepository nodeDataRepository) : base(container)
        {
            _nodeDataRepository = nodeDataRepository;
        }

        public async Task<ServiceResult<NodeCollectionAggregate>> GetNodeData(int nodeId, int pageNumber, int pageSize,
            string[] properties, DateTime from, DateTime to, DataOrder order)
        {
            if (properties.Length == 0 || properties is null) throw new SmartHomeException("No properties selected.");

            var response = new ServiceResult<NodeCollectionAggregate>(Principal);

            var query = _nodeDataRepository.AsQueryableNoTrack()
                .Where(x => x.NodeId == nodeId)
                .Where(x => x.TimeStamp >= from && x.TimeStamp <= to);

            var results = await FilterByDate(query, order).ToListAsync();

            if (!results.Any()) throw new SmartHomeEntityNotFoundException($"Node ID: {nodeId} data not found");

            var aggregate = GetAggregate(results, properties);
            response.Data = aggregate;

            return response;
        }

        private IQueryable<NodeData> FilterByDate(IQueryable<NodeData> query, DataOrder order)
        {
            if (order == DataOrder.Asc) return query.OrderBy(x => x.TimeStamp);
            return query.OrderByDescending(x => x.TimeStamp);
        }

        private static NodeCollectionAggregate GetAggregate(List<NodeData> paginated, string[] properties)
        {
            // Create response
            var aggregate = new NodeCollectionAggregate();

            var timeStamps = paginated
                .Where(nd => nd.Magnitudes
                .Any(ndm => properties.Any(magn => magn == ndm.Magnitude)))
                .Select(x => x.TimeStamp)
                .ToList();

            aggregate.AddTimestamps(timeStamps);

            foreach (var property in properties)
            {
                var nodeDataMagnitudes = paginated
                    .SelectMany(x => x.Magnitudes)
                    .Where(x => x.Magnitude == property);

                if (!nodeDataMagnitudes.Any()) throw new SmartHomeEntityNotFoundException($"Property {property} is not valid");

                var descriptor = new MetadataDescriptor
                {
                    Magnitude = property,
                    Unit = nodeDataMagnitudes.FirstOrDefault()?.Unit,
                    CollectionLength = nodeDataMagnitudes.Count()
                };

                var values = nodeDataMagnitudes.Select(x => x.Value).ToList();

                aggregate.AddProperty<string>(property, values, descriptor);

            }

            return aggregate.MagnitudeDictionary.Count == 0 ? new NodeCollectionAggregate() : aggregate;
        }

        public async Task<NodeData> AddSingleAsync(int nodeId, EDataRequestReason reason, NodeDataMagnitudeDto data)
        {
            var samplesToKeep = Config.GetValue<int>("Defaults:NodeDataRetention:SamplesToKeep");

            return await _nodeDataRepository.AddSingleAsync(nodeId, samplesToKeep, reason, new NodeDataMagnitude
            {
                Magnitude = data.PhysicalProperty.Magnitude,
                Unit = data.PhysicalProperty.Unit,
                Value = data.Value
            });
        }

        public async Task<NodeData> AddManyAsync(int nodeId, EDataRequestReason reason, IEnumerable<NodeDataMagnitudeDto> data)
        {
            var samplesToKeep = Config.GetValue<int>("Defaults:NodeDataRetention:SamplesToKeep");

            return await _nodeDataRepository.AddManyAsync(nodeId, samplesToKeep, reason, data.Select(x =>
                new NodeDataMagnitude
                {
                    Magnitude = x.PhysicalProperty.Magnitude,
                    Unit = x.PhysicalProperty.Unit,
                    Value = x.Value
                }).ToList());
        }
    }
}
