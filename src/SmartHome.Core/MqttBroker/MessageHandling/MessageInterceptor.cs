using SmartHome.Core.Dto;
using SmartHome.Core.MessageHanding;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace SmartHome.Core.MqttBroker.MessageHandling
{
    public class MessageInterceptor
    {
        public bool IsDebugLogEnabled { get; set; } = false;
        private readonly IMessageProcessor<MqttMessageDto> _messageProcessor;

        public MessageInterceptor(IMessageProcessor<MqttMessageDto> processor, IHostingEnvironment env)
        {
            _messageProcessor = processor;
            IsDebugLogEnabled = env.IsDevelopment();
        }

        public MessageInterceptor(bool isDebugLogEnabled)
        {
            IsDebugLogEnabled = isDebugLogEnabled;
        }

        public async Task Intercept(ReceivedMessage message)
        {
            if (IsDebugLogEnabled) LogDebug(message);

            await _messageProcessor.Process(new MqttMessageDto
            {
                ClientId = message.ClientId,
                Topic = message.Topic,
                Payload = message.Payload,
                ContentType = message.ContentType
            });
        }

        private void LogDebug(ReceivedMessage message)
        {
            Debug.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
            Debug.WriteLine($"+ ClientID = {message.ClientId}");
            Debug.WriteLine($"+ Topic = {message.Topic}");
            Debug.WriteLine($"+ ContentType = {message.ContentType}");
            Debug.WriteLine($"+ Payload = {message.Payload}");
            Debug.WriteLine("###");
        }
    }
}
