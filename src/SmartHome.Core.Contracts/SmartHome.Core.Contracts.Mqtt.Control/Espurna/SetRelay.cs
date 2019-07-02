using MQTTnet;
using Newtonsoft.Json.Linq;
using SmartHome.Core.Domain.ContractParams;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.MqttBroker;
using System.Threading.Tasks;

namespace SmartHome.Core.Contracts.Mqtt.Control.Espurna
{
    public class SetRelay : IMqttControlStrategy
    {
        private readonly IMqttService _mqttService;

        public SetRelay(IMqttService mqttService)
        {
            _mqttService = mqttService;
        }

        public async Task Execute(Node node, JObject commandParams)
        {
            var param = commandParams.ToObject<SingleRelayParam>();

            if (string.IsNullOrEmpty(param.State)) throw new SmartHomeException("Relay state cannot be null: missing 'state' key");
            if (string.IsNullOrEmpty(param.RelayNo)) throw new SmartHomeException("Relay number cannot be null: missing 'relayNo' key");
            if (string.IsNullOrEmpty(node.ApiKey)) throw new SmartHomeException("API key cannot be empty");
            if (string.IsNullOrEmpty(node.BaseTopic)) throw new SmartHomeException("Base topic cannot be empty");

            var message = new MqttApplicationMessageBuilder()
                .WithTopic($"{node.BaseTopic}/relay/{param.RelayNo}/set")
                .WithPayload(param.State)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await _mqttService.PublishSystemMessageAsync(message);
        }
    }
}
