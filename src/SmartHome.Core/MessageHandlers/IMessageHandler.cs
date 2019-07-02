using System.Threading.Tasks;
using SmartHome.Core.Domain.Entity;

namespace SmartHome.Core.MessageHandlers
{
    public interface IMessageHandler<T> where T : class, new()
    {
        Task Handle(Node node, T message);
    }
}