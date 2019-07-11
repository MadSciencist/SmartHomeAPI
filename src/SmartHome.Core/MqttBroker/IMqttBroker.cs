using MQTTnet;
using MQTTnet.Client.Publishing;
using MQTTnet.Server;
using MQTTnet.Server.Status;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.MqttBroker
{
    public interface IMqttBroker
    {
        IMqttServerOptions ServerOptions { get; set; }
        Task StartAsync();
        Task StopAsync();
        Task<MqttClientPublishResult> PublishSystemMessageAsync(MqttApplicationMessage message);
        Task<ICollection<IMqttClientStatus>> GetClientStatusAsync();
    }
}