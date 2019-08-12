using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHome.Core.Dto.NodeData
{
    public class NodeComposedData
    {
        public int NodeId { get; set; }

        public ICollection<NodeMagnitudeData> Datas { get; set; }
    }
}
    