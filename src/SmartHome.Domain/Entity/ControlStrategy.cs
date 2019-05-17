using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Domain.Entity
{
    [Table("tbl_control_strategy")]
    public class ControlStrategy : EntityBase
    {
        [Required, MaxLength(50)]
        public string ProviderName { get; set; }

        [Required, MaxLength(50)]
        public string ContextName { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public ICollection<RegisteredSensors> RegisteredSensors { get; set; }

        // Many-to-many relationship
        public ICollection<ControlStrategyCommandLink> AllowedCommands { get; set; }
    }
}
