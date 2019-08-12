using SmartHome.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHome.Core.Dto.NodeData
{
    public class NodeMagnitudeDataPaged : NodeMagnitudeData
    {
        public new PagedResult<NodeMagnitudeRecord> Data { get; set; }
    }
}
