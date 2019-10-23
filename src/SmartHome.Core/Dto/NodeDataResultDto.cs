using SmartHome.Core.Entities.Entity;
using System.Collections.Generic;

namespace SmartHome.Core.Dto
{
    public class NodeDataResultDto
    {
        public IEnumerable<NodeDataRecordDto> Values { get; set; }
        public PhysicalProperty PhysicalProperty { get; set; }
    }
}
