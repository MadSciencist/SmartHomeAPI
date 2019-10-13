using Autofac;
using MQTTnet;
using Newtonsoft.Json.Linq;
using SmartHome.Core.Control;
using SmartHome.Core.Entities.Attributes;
using SmartHome.Core.Entities.Entity;
using System.Threading.Tasks;

namespace SmartHome.Contracts.TasmotaMqtt.Commands
{
    [DisplayText("Get power statistics")]
    [ParameterType(null)]
    public class GetPowerStats : MqttControlCommand, IControlCommand
    {
        public GetPowerStats(ILifetimeScope container, Node node) : base(container, node)
        {
        }

        public async Task Execute(JObject commandParams)
        {
            await EnsureNodeOnline();

            var message = new MqttApplicationMessageBuilder()
                .WithTopic($"{Node.BaseTopic}/status")
                .WithPayload("8")
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await MqttBroker.PublishSystemMessageAsync(message);
        }
    }
}
