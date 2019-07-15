using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Domain.Entity
{
    [Table("tbl_control_strategy")]
    public class ControlStrategy : EntityBase
    {
        [Required, MaxLength(250)]
        public string AssemblyProduct { get; set; }

        [Required, MaxLength(250)]
        public string ContractAssembly { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public ICollection<RegisteredMagnitude> RegisteredMagnitudes { get; set; }

        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public int CreatedById { get; set; }
        public ICollection<Node> Nodes { get; set; }
    }
}
