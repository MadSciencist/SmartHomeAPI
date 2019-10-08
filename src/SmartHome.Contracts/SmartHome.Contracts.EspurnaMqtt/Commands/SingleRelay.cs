using Autofac;
using MQTTnet;
using Newtonsoft.Json.Linq;
using SmartHome.Core.Control;
using SmartHome.Core.Entities.ContractParams;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Infrastructure.Attributes;
using System.Threading.Tasks;

namespace SmartHome.Contracts.EspurnaMqtt.Commands
{
    [DisplayText("Single Relay")]
    [ParameterType(typeof(SingleRelayParam))]
    public class SingleRelay : MqttControlCommand, IControlCommand
    {
        public SingleRelay(ILifetimeScope container) : base(container)
        {
        }

        public async Task Execute(Node node, JObject commandParams)
        {
            var param = commandParams.ToObject<SingleRelayParam>().Validate();

            if (string.IsNullOrEmpty(node.BaseTopic)) throw new SmartHomeException("Base topic cannot be empty");

            var message = new MqttApplicationMessageBuilder()
                .WithTopic($"{node.BaseTopic}/relay/{param.RelayNo}/set")
                .WithPayload(param.State)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await MqttBroker.PublishSystemMessageAsync(message);
        }
    }
}
