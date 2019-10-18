using Matty.Framework;
using Matty.Framework.Enums;
using SmartHome.Core.Dto;
using SmartHome.Core.Dto.NodeData;
using SmartHome.Core.Entities.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Services.Abstractions
{
    public interface INodeDataService : IServiceBase
    {
        /// <summary>
        /// Add new data to repository and use retention policy
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<NodeData> AddSingleAsync(int nodeId, NodeDataMagnitudeDto data);

        /// <summary>
        /// Add new collection of data to repository and use retention policy
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task AddManyAsync(int nodeId, IEnumerable<NodeDataMagnitudeDto> data);

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
        /// <param name="maxCount">Limit count</param>
        /// <param name="paged">Should result be paged</param>
        /// <returns></returns>
        Task<ServiceResult<ICollection<NodeMagnitudeData>>> GetNodeDatas(int nodeId, int pageNumber,
            int pageSize, string[] properties, DateTime from, DateTime to, DataOrder order, int maxCount = 1000, bool paged = false);
    }
}