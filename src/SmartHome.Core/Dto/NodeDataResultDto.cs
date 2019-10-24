using System.Collections.Generic;

namespace SmartHome.Core.Dto
{
    public class NodeDataResultDto
    {
        public IEnumerable<NodeDataRecordDto> Values { get; set; }
        public PhysicalPropertyDto PhysicalProperty { get; set; }
    }
}
