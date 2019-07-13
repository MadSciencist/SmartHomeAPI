using System.Threading.Tasks;

namespace SmartHome.Core.MessageHanding
{
    /// <summary>
    /// Processor if the first class when new message arrive
    /// It should get correct node and resolve current message handler
    /// </summary>
    public interface IMessageProcessor<in T> where T : class, new()
    {
        Task Process(T message);
    }
}
