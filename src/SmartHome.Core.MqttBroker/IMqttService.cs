using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client.Publishing;
using MQTTnet.Server;

namespace SmartHome.Core.MqttBroker
{
    public interface IMqttService
    {
        IMqttServerOptions ServerOptions { get; set; }
        Task Start();
        Task Stop();
        Task<MqttClientPublishResult> PublishSystemMessageAsync(MqttApplicationMessage message);
        void Log();
    }
}