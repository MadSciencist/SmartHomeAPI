using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Utils;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using SmartHome.Core.Infrastructure;

namespace SmartHome.Core.BusinessLogic
{
    public class NodeDataService : INodeDataService
    {
        public ClaimsPrincipal ClaimsOwner { get; set; }

        private readonly INodeDataRepository _nodeDataRepository;

        public NodeDataService(INodeDataRepository nodeDataRepository)
        {
            _nodeDataRepository = nodeDataRepository;
        }

        public async Task<NodeData> AddSingleAsync(EDataRequestReason reason, NodeDataMagnitudes data)
        {
            int userId = Convert.ToInt32(ClaimsPrincipalHelper.GetClaimedIdentifier(ClaimsOwner));
            if(userId == 0) throw new ArgumentException("user not auth");
            return await _nodeDataRepository.AddSingleAsync(reason, data);
        }

        public async Task<NodeData> AddManyAsync(EDataRequestReason reason, ICollection<NodeDataMagnitudes> data)
        {
            return await _nodeDataRepository.AddManyAsync(reason, data);
        }
    }
}
