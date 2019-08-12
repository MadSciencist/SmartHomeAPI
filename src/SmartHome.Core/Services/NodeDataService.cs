using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Core.DataAccess;
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
        private readonly INodeDataMagnitudeRepository _nodeDataMagnitudeRepository;
        private readonly IServiceProvider _serviceProvider;

        public NodeDataService(ILifetimeScope container, IServiceProvider serviceProvider, INodeDataRepository nodeDataRepository, INodeDataMagnitudeRepository nodeDataMagnitudeRepository) : base(container)
        {
            _nodeDataRepository = nodeDataRepository;
            _nodeDataMagnitudeRepository = nodeDataMagnitudeRepository;
            _serviceProvider = serviceProvider;
        }

        public async Task<ServiceResult<NodeCollectionAggregate>> GetNodeData(int nodeId, int pageNumber, int pageSize,
            string[] properties, DateTime from, DateTime to, DataOrder order)
        {
            if (properties.Length == 0 || properties is null) throw new SmartHomeException("No properties selected.");

            var response = new ServiceResult<NodeCollectionAggregate>(Principal);

            var query = _nodeDataRepository.AsQueryableNoTrack()
                .Where(x => x.NodeId == nodeId)
                .Where(x => x.TimeStamp >= from && x.TimeStamp <= to);

            var results = FilterByDate(query, order).ToListAsync();

            var paginated = await PaginatedList<NodeData>.CreateAsync(query, FilterByDate, pageNumber, pageSize, order);

            if (!paginated.Any()) throw new SmartHomeEntityNotFoundException($"Node ID: {nodeId} data not found");

            response.Data = GetAggregate(paginated, properties);

            return response;
        }

        public async Task<ServiceResult<NodeComposedData>> GetNodeDatas(int nodeId, int pageNumber, int pageSize,
            string[] properties, DateTime from, DateTime to, DataOrder order, bool paged = false)
        {
            if (properties.Length == 0 || properties is null) throw new SmartHomeException("No properties selected.");

            var response = new ServiceResult<NodeComposedData>(Principal);

            //var query = _nodeDataRepository.AsQueryableNoTrack()
            //    .Where(x => x.NodeId == nodeId)
            //    .Where(x => x.TimeStamp >= from && x.TimeStamp <= to)
            //    .SelectMany(x => x.Magnitudes)
            //    .Where(x => properties.Any(p => p == x.Magnitude))
            //    .GroupBy(x => x.Magnitude);

            var tasks = properties.Select(p => Task.Run(() => GetNodeMagnitudeData(nodeId, pageNumber, pageSize, p, from, to, order, paged))).ToArray();

            await Task.WhenAll(tasks);

            response.Data = new NodeComposedData
            {
                Datas = tasks.Select(t => t.Status == TaskStatus.RanToCompletion ? t.Result : throw new Exception("Task Not Completed Successfully")).ToList(),
                NodeId = nodeId
            };

            return response;
        }

        private async Task<NodeMagnitudeData> GetNodeMagnitudeData(int nodeId, int pageNumber, int pageSize,
            string property, DateTime from, DateTime to, DataOrder order, bool paged = false)
        {
            // Using repository brings problems as you cannot run asynchronus operations on THE SAME INSTANCE OF CONTEXT
            var query = _nodeDataMagnitudeRepository.AsQueryableNoTrack()
                .Where(x => x.NodeData.NodeId == nodeId)
                .Where(x => x.NodeData.TimeStamp >= from && x.NodeData.TimeStamp <= to)
                .Where(x => x.Magnitude == property)
                .Select(x => new NodeMagnitudeRecord { TimeStamp = x.NodeData.TimeStamp, Value = x.Value });


            // Resolve App Db
            //using (var context = (AppDbContext)_serviceProvider.GetService(typeof(AppDbContext)))
            //{
            //    var query = context.DataMagnitudes.AsNoTracking()
            //        .Where(x => x.NodeData.NodeId == nodeId)
            //        .Where(x => x.NodeData.TimeStamp >= from && x.NodeData.TimeStamp <= to)
            //        .Where(x => x.Magnitude == property)
            //        .Select(x => new NodeMagnitudeRecord { TimeStamp = x.NodeData.TimeStamp, Value = x.Value });

            var sql = query.ToSql();
                Console.WriteLine($"Generated SQL: {sql}");

                if (paged)
                {
                    return new NodeMagnitudeDataPaged
                    {
                        Magnitude = property,
                        Data = await query.GetPagedAsync(pageNumber, pageSize).ConfigureAwait(false)
                    };
                }

                return new NodeMagnitudeData
                {
                    Magnitude = property,
                    Data = await query.ToListAsync().ConfigureAwait(false)
                };
            //}
        }


        private IQueryable<NodeData> FilterByDate(IQueryable<NodeData> query, DataOrder order)
        {
            if (order == DataOrder.Asc) return query.OrderBy(x => x.TimeStamp);
            return query.OrderByDescending(x => x.TimeStamp);
        }

        private static NodeCollectionAggregate GetAggregate(PaginatedList<NodeData> paginated, string[] properties)
        {
            // Create response
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
