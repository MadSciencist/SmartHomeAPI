using System.Threading.Tasks;
using MQTTnet;
using Newtonsoft.Json.Linq;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.MqttBroker;

namespace SmartHome.Core.Contracts.Mqtt.Control.Espurna
{
    public class SetRelay : IMqttControlStrategy
    {
        private readonly IMqttService _mqttService;

        public SetRelay(IMqttService mqttService)
        {
            _mqttService = mqttService;
        }

        /// <summary>
        /// Example commandParams: 
        /// {
        //	    "relayNo": 0,
        //	    "state": 1
        //  }
        /// </summary>
        /// <param name="node">Target node</param>
        /// <param name="command">Control command</param>
        /// <param name="commandParams">Params from request body</param>
        /// <returns>RAW espurna response</returns>
        public async Task<object> Execute(Node node, JObject commandParams)
        {
            var relayNo = commandParams.Value<string>("relayNo");
            var relayState = commandParams.Value<string>("state");

            if (string.IsNullOrEmpty(relayState)) throw new SmartHomeException("Relay state cannot be null: missing 'state' key");
            if (string.IsNullOrEmpty(relayNo)) throw new SmartHomeException("Relay number cannot be null: missing 'relayNo' key");
            if (string.IsNullOrEmpty(node.ApiKey)) throw new SmartHomeException("API key cannot be empty");
            if (string.IsNullOrEmpty(node.BaseTopic)) throw new SmartHomeException("Base topic cannot be empty");

            var message = new MqttApplicationMessageBuilder()
                .WithTopic($"{node.BaseTopic}/relay/{relayNo}/set")
                .WithPayload(relayState)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

             var result = await _mqttService.PublishSystemMessageAsync(message);
             return new {publishResult = result.ReasonCode.ToString()};
        }
    }
}
