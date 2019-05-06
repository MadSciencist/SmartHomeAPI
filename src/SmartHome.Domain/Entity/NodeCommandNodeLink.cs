using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Domain.Entity
{
    [Table("node_command_link")]
    public class NodeCommandNodeLink : EntityBase
    {
        public int NodeId { get; set; }
        public int NodeCommandId { get; set; }
        public NodeCommand NodeCommand { get; set; }
        public Node Node { get; set; }
    }
}