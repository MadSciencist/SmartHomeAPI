using SmartHome.Core.Dto;
using SmartHome.Core.MessageHanding;
using System.Threading.Tasks;

namespace SmartHome.Core.MqttBroker.MessageHandling
{
    public class MessageInterceptor
    { 
        private readonly IMessageProcessor<MqttMessageDto> _messageProcessor;

        public MessageInterceptor(IMessageProcessor<MqttMessageDto> processor)
        {
            _messageProcessor = processor;
        }

        public async Task Intercept(ReceivedMessage message)
        {
            await _messageProcessor.Process(new MqttMessageDto
            {
                ClientId = message.ClientId,
                Topic = message.Topic,
                Payload = message.Payload,
                ContentType = message.ContentType
            });
        }
    }
}
