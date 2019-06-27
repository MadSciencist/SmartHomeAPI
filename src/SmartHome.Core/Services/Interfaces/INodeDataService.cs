﻿using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Dto;
using SmartHome.Core.Dto.NodeData;
using SmartHome.Core.Infrastructure;
using System;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public interface INodeDataService : IServiceBase
    {
        Task<NodeData> AddSingleAsync(int nodeId, EDataRequestReason reason, NodeDataMagnitudeDto data);
        Task<ServiceResult<NodeCollectionAggregate>> GetNodeData(int nodeId, int pageNumber, int pageSize, string[] properties, DateTime from, DateTime to, DataOrder order);
    }
}