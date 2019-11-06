using Matty.Framework;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Entity;
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
        Task<NodeData> AddSingleAsync(int nodeId, NodeDataDto data);

        /// <summary>
        /// Add new collection of data to repository and use retention policy
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task AddManyAsync(int nodeId, IEnumerable<NodeDataDto> data);

        /// <summary>
        /// Query for stored NodeData.
        /// Ordered by date descending.
        /// </summary>
        /// <param name="nodeId">IF od the node</param>
        /// <param name="magnitude">Physical's property magnitude</param>
        /// <param name="limit">Limit of results</param>
        /// <returns></returns>
        Task<ServiceResult<NodeDataResultDto>> GetNodeDataByMagnitude(int nodeId, string magnitude, int limit);

        /// <summary>
        /// Gets last seen datetime of each node.
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<NodeLastSeenDto>>> GetNodesLastSeen();
    }
}