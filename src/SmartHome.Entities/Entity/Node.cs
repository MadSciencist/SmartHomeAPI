using SmartHome.Core.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace SmartHome.Core.Entities.Entity
{
    [Table("tbl_node")]
    public class Node : EntityBase
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(10)]
        public string UriSchema { get; set; }

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

        /// <summary>
        /// Checks whether magnitude should be saved in DB
        /// </summary>
        /// <param name="magnitude"></param>
        /// <returns></returns>
        public bool ShouldMagnitudeBeStored(string magnitude)
        {
            return ControlStrategy.RegisteredMagnitudes.Any(x => string.Compare(x.Magnitude, magnitude, StringComparison.InvariantCultureIgnoreCase) == 0);
        }
        
        /// <summary>
        /// Check whether node is online by sending ICMP packet
        /// Uses configurable timeout via appSettings
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsPingable()
        {
            if (string.IsNullOrEmpty(IpAddress))
                throw new NullReferenceException("Attempting to ping node which doesn't have IP address provided");

            // TODO get configuration here somehow
            var timeout = 1000;

            using (var ping = new Ping())
            {
                var result = await ping.SendPingAsync(IpAddress, timeout, new byte[1]);
                return result.Status == IPStatus.Success;
            }
        }

        /// <summary>
        /// Returns node name
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;
    }
}
