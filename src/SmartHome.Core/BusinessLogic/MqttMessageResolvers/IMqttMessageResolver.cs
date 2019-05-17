using System.Threading.Tasks;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Dto;

namespace SmartHome.Core.BusinessLogic.MqttMessageResolvers
{
    // all the resolvers should be in this namespace
    public interface IMqttMessageResolver
    {
        Task Resolve(Node node, MqttMessageDto messageDto);
    }
}
