using System.Collections.Generic;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client.Publishing;
using MQTTnet.Server;
using MQTTnet.Server.Status;

namespace SmartHome.Core.MqttBroker
{
    public interface IMqttService
    {
        IMqttServerOptions ServerOptions { get; set; }
        Task StartBroker();
        Task StopBroker();
        Task<MqttClientPublishResult> PublishSystemMessageAsync(MqttApplicationMessage message);
        Task<ICollection<IMqttClientStatus>> GetClientStatusAsync();
    }
}