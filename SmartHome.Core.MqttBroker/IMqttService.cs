using System.Threading.Tasks;
using MQTTnet.Server;

namespace SmartHome.Core.MqttBroker
{
    public interface IMqttService
    {
        IMqttServerOptions ServerOptions { get; set; }
        Task Start();
        Task Stop();
        void Log();
    }
}