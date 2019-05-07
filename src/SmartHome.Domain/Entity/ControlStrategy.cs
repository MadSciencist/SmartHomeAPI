using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Domain.Entity
{
    [Table("control_strategy")]
    public class ControlStrategy : EntityBase
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string ExecutorClassNamespace { get; set; }

        [Required, MaxLength(20)]
        public ControlStrategyType Type { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        // Many-to-many relationship
        public ICollection<ControlStrategyCommandLink> AllowedCommands { get; set; }
    }

    public enum ControlStrategyType : byte
    {
        Rest = 0,
        Mqtt = 100
    }
}
