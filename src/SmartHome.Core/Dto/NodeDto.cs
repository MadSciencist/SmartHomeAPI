using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartHome.Core.Dto
{
    public class NodeDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string GatewayIpAddress { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string ApiKey { get; set; }
        public string BaseTopic { get; set; }
        public string ClientId { get; set; }
        public string ConfigMetadata { get; set; }
        public string ControlStrategyName { get; set; }
        public ICollection<string> Magnitudes { get; set; }
        public int CreatedById { get; set; }
    }
}
