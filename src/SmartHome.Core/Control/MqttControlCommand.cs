using Autofac;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.MqttBroker;

namespace SmartHome.Core.Control
{
    public abstract class MqttControlCommand : ControlCommandBase
    {
        protected MqttControlCommand(ILifetimeScope container, Node node) : base(container, node)
        {
        }

        private IMqttBroker _mqttBroker;
        protected IMqttBroker MqttBroker =>
            _mqttBroker ?? (_mqttBroker = Container.Resolve<IMqttBroker>());
    }
}
