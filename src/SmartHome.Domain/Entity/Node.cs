using SmartHome.Core.Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SmartHome.Core.Domain.Entity
{
    [Table("tbl_node")]
    public class Node : EntityBase
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(20)]
        public string IpAddress { get; set; }

        [Range(80, 99999)]
        public int Port { get; set; }

        [MaxLength(20)]
        public string GatewayIpAddress { get; set; }

        [MaxLength(40)]
        public string Login { get; set; }

        [MaxLength(40)]
        public string Password { get; set; }

        [MaxLength(30)]
        public string ApiKey { get; set; }

        [MaxLength(100)]
        public string BaseTopic { get; set; }

        [MaxLength(100)]
        public string ClientId { get; set; }

        [MaxLength(Int32.MaxValue)]
        public string ConfigMetadata { get; set; }

        public ICollection<NodeData> NodeData { get; set; }

        // Navigation & relationship properties
        public ControlStrategy ControlStrategy { get; set; }
        public int? ControlStrategyId { get; set; }

        public int CreatedById { get; set; }
        public AppUser CreatedBy { get; set; }
        public ICollection<AppUserNodeLink> AllowedUsers { get; set; }

        public DateTime Created { get; set; }

        public bool UserWantsToSaveProperty(string magnitude)
        {
            return ControlStrategy.RegisteredMagnitudes.Any(x => string.Compare(x.Magnitude, magnitude, StringComparison.InvariantCultureIgnoreCase) == 0);
        }
    }
}
