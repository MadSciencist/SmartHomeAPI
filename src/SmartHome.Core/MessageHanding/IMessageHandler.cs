using System.Threading.Tasks;

namespace SmartHome.Core.MessageHanding
{
    /// <summary>
    /// Message handler should perform action like save it to repository or notify clients
    /// with context to particular message and node
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessageHandler<in T> where T : class, new()
    {
        Task Handle(T message);
    }
}