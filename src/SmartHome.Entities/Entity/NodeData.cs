using Matty.Framework;
using Matty.Framework.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Entities.Entity
{
    [Table("tbl_node_data")]
    public class NodeData : EntityBase<int>
    {
        public DateTime TimeStamp { get; set; }

        // Navigation properties
        public ICollection<NodeDataMagnitude> Magnitudes { get; set; }

        public int NodeId { get; set; }
        public Node Node { get; set; }
    }
}
