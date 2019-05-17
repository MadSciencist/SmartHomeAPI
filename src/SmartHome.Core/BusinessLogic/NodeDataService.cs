using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Dto;
using SmartHome.Core.Utils;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

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

        public async Task<NodeData> AddSingleAsync(EDataRequestReason reason, NodeDataDto data)
        {
            if (reason == EDataRequestReason.User)
            {
                // TODO validation
                int userId = Convert.ToInt32(ClaimsPrincipalHelper.GetClaimedIdentifier(ClaimsOwner));
                if (userId == 0) throw new ArgumentException("user not auth");
            }

            return await _nodeDataRepository.AddSingleAsync(reason, new NodeDataMagnitude
            {
                Magnitude = data.Magnitude,
                Unit = data.Unit,
                Value = data.Value
            });
        }

        // todo change argument to DTO
        public async Task<NodeData> AddManyAsync(EDataRequestReason reason, ICollection<NodeDataMagnitude> data)
        {
            return await _nodeDataRepository.AddManyAsync(reason, data);
        }
    }
}
