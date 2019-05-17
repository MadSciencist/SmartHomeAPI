using System.Threading.Tasks;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Dto;

namespace SmartHome.Core.BusinessLogic.TopicResolvers
{
    public interface ITopicResolver
    {
        Task Resolve(Node node, MqttMessageDto messageDto);
    }
}
