using Autofac;
using SmartHome.Core.MqttBroker;

namespace SmartHome.Core.Control
{
    public abstract class MqttControlCommand : ControlCommandBase
    {
        protected MqttControlCommand(ILifetimeScope container) : base(container)
        {
        }

        private IMqttBroker _mqttBroker;
        protected IMqttBroker MqttBroker =>
            _mqttBroker ?? (_mqttBroker = Container.Resolve<IMqttBroker>());
    }
}
