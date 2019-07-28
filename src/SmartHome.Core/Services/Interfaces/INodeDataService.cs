using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Dto;
using SmartHome.Core.Dto.NodeData;
using SmartHome.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public interface INodeDataService : IServiceBase
    {
        /// <summary>
        /// Add new data to repository and use retention policy
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="reason"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<NodeData> AddSingleAsync(int nodeId, EDataRequestReason reason, NodeDataMagnitudeDto data);

        /// <summary>
        /// Add new collection of data to repository and use retention policy
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="reason"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<NodeData> AddManyAsync(int nodeId, EDataRequestReason reason,
            IEnumerable<NodeDataMagnitudeDto> data);

        /// <summary>
        /// Query data repository
        /// </summary>
        /// <param name="nodeId">ID of the node</param>
        /// <param name="pageNumber">Paging: number of current page</param>
        /// <param name="pageSize">Paging: items per page</param>
        /// <param name="properties">Collection of properties to retrieve</param>
        /// <param name="from">Starts date</param>
        /// <param name="to">Ends date</param>
        /// <param name="order">Order data by timestamps (ASC or DESC)</param>
        /// <returns></returns>
        Task<ServiceResult<NodeCollectionAggregate>> GetNodeData(int nodeId, int pageNumber,
            int pageSize, string[] properties, DateTime from, DateTime to, DataOrder order);
    }
}