using Matty.Framework;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Entities.Entity
{
    [Table("tbl_node_data")]
    public class NodeData : EntityBase<int>
    {
        public DateTime TimeStamp { get; set; }
        public string Value { get; set; }
        public int NodeId { get; set; }
        public Node Node { get; set; }
        public int PhysicalPropertyId { get; set; }
        public PhysicalProperty PhysicalProperty { get; set; }
    }
}
