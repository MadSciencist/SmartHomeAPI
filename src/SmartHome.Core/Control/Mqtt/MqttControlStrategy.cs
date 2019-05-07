using System;
using System.Threading.Tasks;
using SmartHome.Domain.Entity;

namespace SmartHome.Core.Control.Mqtt
{
    public class MqttControlStrategy : IControlStrategy
    {
        public async Task<object> Execute(Node node, Command command)
        {
            throw new NotImplementedException();
        }
    }
}