using System.Threading.Tasks;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Dto;

namespace SmartHome.Core.MessageHandlers
{
    public interface IMqttMessageHandler
    {
        Task Handle(Node node, MqttMessageDto messageDto);
    }
}
