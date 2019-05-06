﻿using SmartHome.Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Domain.Entity
{
    [Table("node")]
    public class Node : EntityBase
    {
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(20)]
        public string IpAddress { get; set; }

        [Range(80, 99999)]
        public int Port { get; set; }

        [MaxLength(20)]
        public string GatewayIpAddress { get; set; }

        [MaxLength(30)]
        public string ApiKey { get; set; }

        // Many-to-many relationship
        public ICollection<NodeCommandNodeLink> AllowedCommands { get; set; }


        // Navigation & relationship properties
        public ControlStrategy ControlStrategy { get; set; }
        public int ControlStrategyId { get; set; }

        public int CreatedById { get; set; }
        public AppUser CreatedBy { get; set; }
        public ICollection<AppUserNodeLink> AllowedUsers { get; set; }

        public DateTime Created { get; set; }
    }
}
