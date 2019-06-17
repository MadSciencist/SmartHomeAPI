using System;
using System.Collections.Generic;
using Autofac;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using SmartHome.Core.Dto.NodeData;

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
            string[] properties, DateTime from, DateTime to)
        {
            var response = new ServiceResult<NodeCollectionAggregate>(Principal);

            var queryable = _nodeDataRepository.AsQueryableNoTrack()
                .Where(x => x.NodeId == nodeId)
                .Where(x => x.TimeStamp >= from && x.TimeStamp <= to)
                .OrderBy(x => x.TimeStamp);

            var paginated = await PaginatedList<NodeData>.CreateAsync(queryable, pageNumber, pageSize);

            response.Data = GetAggregate(paginated, properties);

            return response;
        }

        private static NodeCollectionAggregate GetAggregate(PaginatedList<NodeData> paginated, string[] properties)
        {
            var firstValueSet = paginated.First();

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

            foreach (var magnitude in firstValueSet.Magnitudes)
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

        public async Task<NodeData> AddSingleAsync(int nodeId, EDataRequestReason reason, NodeDataMagnitudeDto data)
        {
            // TODO Do some retention - collect only 100k last samples or smth
            return await _nodeDataRepository.AddSingleAsync(nodeId, reason, new NodeDataMagnitude
            {
                Magnitude = data.Magnitude,
                Unit = data.Unit,
                Value = data.Value
            });
        }
    }
}
