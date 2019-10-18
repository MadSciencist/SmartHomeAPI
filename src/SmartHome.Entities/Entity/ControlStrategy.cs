using Matty.Framework;
using Matty.Framework.Abstractions;
using SmartHome.Core.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Entities.Entity
{
    [Table("tbl_control_strategy")]
    public class ControlStrategy : EntityBase<int>, ICreationAudit<AppUser, int>, IModificationAudit<AppUser, int?>
    {
        [Required, MaxLength(250)]
        public string AssemblyProduct { get; set; }

        [Required, MaxLength(250)]
        public string ContractAssembly { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        // Navigation & relationship properties
        public ICollection<Node> Nodes { get; set; }

        public ICollection<RegisteredMagnitude> RegisteredMagnitudes { get; set; }

        #region ICreationAudit impl
        public int CreatedById { get; set; }
        public AppUser CreatedBy { get; set; }
        public DateTime Created { get; set; }
        #endregion

        #region IModificationAudit impl
        public int? UpdatedById { get; set; }
        public AppUser UpdatedBy { get; set; }
        public DateTime? Updated { get; set; }
        #endregion

        /// <summary>
        /// Gets fully qualified name of command executor class
        /// Convention is SmartHome.Core.Contracts.{name}.Commands.Command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public string GetCommandFullyQuallifiedName(string command)
        {
            return ContractAssembly.Split(".dll")[0] + ".Commands." + command;
        }
    }
}
