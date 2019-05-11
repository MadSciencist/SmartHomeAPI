using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Domain.Entity
{
    [Table("control_strategy")]
    public class ControlStrategy : EntityBase
    {
        [Required, MaxLength(50)]
        public string ProviderName { get; set; }

        [Required, MaxLength(50)]
        public string ContextName { get; set; }

        //[Required, MaxLength(100)]
        //public string ExecutorClassNamespace { get; set; } // not needed ? get asm by reflection then join with context and class name

        [MaxLength(250)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        // Many-to-many relationship
        public ICollection<ControlStrategyCommandLink> AllowedCommands { get; set; }
    }
}
