using System;
using System.Collections.Generic;

namespace SmartHome.Core.Dto
{
    public class NodeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UriSchema { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string GatewayIpAddress { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string ApiKey { get; set; }
        public string BaseTopic { get; set; }
        public string ClientId { get; set; }
        public string ConfigMetadata { get; set; }
        public int ControlStrategyId { get; set; }
        public IEnumerable<PhysicalPropertyDto> PhysicalProperties { get; set; }
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public int? LastUpdatedById { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? LastSeen { get; set; }
    }
}
