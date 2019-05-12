using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Domain.Entity
{
    [Table("tbl_node_data")]
    public class NodeData : EntityBase
    {
        public DateTime TimeStamp { get; set; }
        public ICollection<NodeDataMagnitudes> Magnitudes { get; set; }

        public int NodeId { get; set; }
        public Node Node { get; set; }

        public int RequestReasonId { get; set; }
        public DataRequestReason RequestReason { get; set; }
    }
}
