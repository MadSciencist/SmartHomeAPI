﻿namespace SmartHome.Core.Dto
{
    public class CreateNodeDto
    {
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
    }
}
