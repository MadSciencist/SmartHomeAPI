using System;

namespace SmartHome.Core.Dto
{
    public class NodeLastSeenDto
    {
        public int NodeId { get; set; }
        public DateTime? LastSeen { get; set; }
    }
}
