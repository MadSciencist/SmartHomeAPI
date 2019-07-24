using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Domain.Entity
{
    [Table("tbl_node_data_magnitude")]
    public class NodeDataMagnitude : EntityBase
    {
        public string Magnitude { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }

        // Navigation properties
        public int NodeDataId { get; set; }
        public NodeData NodeData { get; set; }
    }
}
