using Autofac;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Dto;
using SmartHome.Core.Dto.NodeData;
using SmartHome.Core.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

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
            var response = new ServiceResult<NodeCollectionAggregate>(Principal);

            var query = _nodeDataRepository.AsQueryableNoTrack()
                .Where(x => x.NodeId == nodeId)
                .Where(x => x.TimeStamp >= from && x.TimeStamp <= to);
            
            var paginated = await PaginatedList<NodeData>.CreateAsync(query, FilterByDate, pageNumber, pageSize, order);

            if (paginated.FirstOrDefault() == null) throw new SmartHomeEntityNotFoundException($"Node ID: {nodeId} data not found");

            response.Data = GetAggregate(paginated, properties);

            return response;
        }

        private IQueryable<NodeData> FilterByDate(IQueryable<NodeData> query, DataOrder order)
        {
            if (order == DataOrder.Asc) return query.OrderBy(x => x.TimeStamp);
            return query.OrderByDescending(x => x.TimeStamp);
        }

        private static NodeCollectionAggregate GetAggregate(PaginatedList<NodeData> paginated, string[] properties)
        {
            var firstValueSet = paginated.FirstOrDefault();

            var aggregate = new NodeCollectionAggregate
            {
                Pagination = new PaginationResult
                {
                    HasNextPage = paginated.HasNextPage,
                    HasPreviousPage = paginated.HasPreviousPage,
                    PageIndex = paginated.PageIndex,
                    TotalCount = paginated.TotalCount,
                    TotalPages = paginated.TotalPages
                }
            };

            aggregate.AddTimestamps(paginated.Select(x => x.TimeStamp).ToList());

            foreach (var magnitude in firstValueSet?.Magnitudes)
            {
                // Try skip if current magnitude is not in user request
                if (!properties.Any(x => x == magnitude.Magnitude))
                {
                    // Don't skip if user want all properties (or the properties array is empty ( ?properties=))
                    if (!properties.Any(x => x == "all") && properties.Length != 0 && properties[0] != null)
                    {
                        continue;
                    }
                }

                var magnitudeValues = paginated.SelectMany(x => x.Magnitudes).Select(x => x.Value).ToList();
                if (magnitudeValues.Count > 0)
                {
                    var descriptor = new MetadataDescriptor
                    {
                        Magnitude = magnitude.Magnitude,
                        Unit = magnitude.Unit,
                        CollectionLength = magnitudeValues.Count
                    };

                    aggregate.AddProperty<string>(magnitude.Magnitude, magnitudeValues, descriptor);
                }
            }

            return aggregate.MagnitudeDictionary.Count == 0 ? new NodeCollectionAggregate() : aggregate;
        }

        public async Task<NodeData> AddSingleAsync(int nodeId, EDataRequestReason reason,
                                                   NodeDataMagnitudeDto data)
        {
            var samplesToKeep = Config.GetValue<int>("Defaults:NodeDataRetention:SamplesToKeep");

            return await _nodeDataRepository.AddSingleAsync(nodeId, samplesToKeep, reason, new NodeDataMagnitude
            {
                Magnitude = data.PhysicalProperty.Magnitude,
                Unit = data.PhysicalProperty.Unit,
                Value = data.Value
            });
        }
    }
}
