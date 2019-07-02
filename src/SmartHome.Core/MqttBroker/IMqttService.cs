using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Client.Publishing;
using MQTTnet.Server;
using MQTTnet.Server.Status;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.MqttBroker
{
    public interface IMqttService
    {
        IMqttServer Server { get; }
        IMqttServerOptions Options { get; }

        Task<MqttClientPublishResult> PublishSystemMessageAsync(MqttApplicationMessage message);
        Task<ICollection<IMqttClientStatus>> GetClientStatusAsync();
    }
}