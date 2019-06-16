using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartHome.Core.Domain.User;

namespace SmartHome.Core.Domain.Entity
{
    [Table("tbl_control_strategy")]
    public class ControlStrategy : EntityBase
    {
        [Required, MaxLength(50)]
        public string ControlProviderName { get; set; }

        [Required, MaxLength(50)]
        public string ControlContext { get; set; }

        [Required, MaxLength(50)]
        public string ReceiveProviderName { get; set; }

        [Required, MaxLength(50)]
        public string ReceiveContext { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public bool IsActive { get; set; }
        public int CreatedById { get; set; }
        public AppUser CreatedBy { get; set; }
        public DateTime Created { get; set; }

        public ICollection<Node> Nodes { get; set; }
        public ICollection<ControlStrategyLinkage> ControlStrategyLinkages { get; set; }

        [Obsolete]
        public ICollection<RegisteredSensors> RegisteredSensors { get; set; }
        [Obsolete]
        // Many-to-many relationship
        public ICollection<ControlStrategyCommandLink> AllowedCommands { get; set; }
    }
}
