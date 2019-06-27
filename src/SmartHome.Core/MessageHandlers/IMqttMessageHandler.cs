using SmartHome.Core.Dto;

namespace SmartHome.Core.MessageHandlers
{
    public interface IMqttMessageHandler : IMessageHandler<MqttMessageDto>
    {
    }
}
