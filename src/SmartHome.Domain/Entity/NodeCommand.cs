using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Domain.Entity
{
    [Table("node_commands")]
    public class NodeCommand : EntityBase
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string BaseUri { get; set; }
        public string Value { get; set; }
        public ICollection<NodeCommandNodeLink> Nodes { get; set; }
    }
}
