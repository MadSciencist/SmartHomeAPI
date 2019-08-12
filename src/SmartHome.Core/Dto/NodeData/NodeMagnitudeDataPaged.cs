using SmartHome.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHome.Core.Dto.NodeData
{
    public class NodeMagnitudeDataPaged : NodeMagnitudeData
    {
        private PagedResult<NodeMagnitudeRecord> _pagedData;
        public PagedResult<NodeMagnitudeRecord> PagedData {
            get => _pagedData;
            set
            {
                _pagedData = value;
                Data = value.Results;
            }
        }
    }
}
