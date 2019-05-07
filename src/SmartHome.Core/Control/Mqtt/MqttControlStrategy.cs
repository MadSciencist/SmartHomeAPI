using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SmartHome.Domain.Entity;

namespace SmartHome.Core.Control.Mqtt
{
    public class MqttControlStrategy : IControlStrategy
    {
        public Task<object> Execute(Node node, Command command, JObject commandParams)
        {
            throw new NotImplementedException();
        }
    }
}