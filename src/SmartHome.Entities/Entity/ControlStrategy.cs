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
        /// <summary>
        /// The name of control strategy. Is indicating the device type.
        /// </summary>
        [Required, MaxLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// Description of the control strategy.
        /// </summary>
        [MaxLength(250)]
        public string Description { get; set; }

        /// <summary>
        /// This links the strategy to the contract DLL (to Assembly Product).
        /// </summary>
        [Required, MaxLength(250)]
        public string Connector { get; set; }

        /// <summary>
        /// Fully quallified name of the contract assembly.
        /// </summary>
        [Required, MaxLength(250)]
        public string ContractAssembly { get; set; }

        /// <summary>
        /// Indicating whether is active and can be used for new entities.
        /// </summary>
        public bool IsActive { get; set; }

        // Navigation & relationship properties
        /// <summary>
        /// Nodes relationship property.
        /// </summary>
        public IEnumerable<Node> Nodes { get; set; }

        /// <summary>
        /// Many-to-many relationship with PhysicalProperty
        /// </summary>
        public IEnumerable<PhysicalPropertyControlStrategyLink> PhysicalProperties { get; set; }

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
