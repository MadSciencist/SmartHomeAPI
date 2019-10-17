using Autofac;
using MQTTnet;
using Newtonsoft.Json.Linq;
using SmartHome.Core.Abstractions;
using SmartHome.Core.Control;
using SmartHome.Core.Entities.Attributes;
using SmartHome.Core.Entities.ContractParams;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Infrastructure.Exceptions;
using System.Threading.Tasks;

namespace SmartHome.Contracts.EspurnaMqtt.Commands
{
    [DisplayText("Single Relay")]
    [ParameterType(typeof(SingleRelayParam))]
    public class SingleRelay : MqttControlCommand, IControlCommand
    {
        public SingleRelay(ILifetimeScope container, Node node) : base(container, node)
        {
        }

        public async Task Execute(JObject commandParams)
        {
            var param = commandParams.ToObject<SingleRelayParam>().Validate();

            if (string.IsNullOrEmpty(Node.BaseTopic)) throw new SmartHomeException("Base topic cannot be empty");

            var message = new MqttApplicationMessageBuilder()
                .WithTopic($"{Node.BaseTopic}/relay/{param.RelayNo}/set")
                .WithPayload(param.State)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await MqttBroker.PublishSystemMessageAsync(message);
        }
    }
}
