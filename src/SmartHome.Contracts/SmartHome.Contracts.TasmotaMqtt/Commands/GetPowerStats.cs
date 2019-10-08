using Autofac;
using MQTTnet;
using Newtonsoft.Json.Linq;
using SmartHome.Core.Control;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Infrastructure.Attributes;
using System.Threading.Tasks;

namespace SmartHome.Contracts.TasmotaMqtt.Commands
{
    [DisplayText("Get power statistics")]
    [ParameterType(null)]
    public class GetPowerStats : MqttControlCommand, IControlCommand
    {
        public GetPowerStats(ILifetimeScope container) : base(container)
        {
        }

        public async Task Execute(Node node, JObject commandParams)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic($"{node.BaseTopic}/status")
                .WithPayload("8")
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await MqttBroker.PublishSystemMessageAsync(message);
        }
    }
}
