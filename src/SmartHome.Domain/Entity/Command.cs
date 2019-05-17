using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Domain.Entity
{
    [Table("tbl_command")]
    public class Command : EntityBase
    {
        [Required, MaxLength(50)]
        public string Alias { get; set; }

        [Required, MaxLength(100)]
        public string ExecutorClassName { get; set; }

        public ICollection<ControlStrategyCommandLink> Nodes { get; set; }
    }
}
