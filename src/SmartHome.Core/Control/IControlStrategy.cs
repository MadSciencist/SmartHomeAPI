using SmartHome.Domain.Entity;
using System.Threading.Tasks;

namespace SmartHome.Core.Control
{
    public interface IControlStrategy
    {
        Task<object> Execute(Node node, ControlCommand command);
    }
}
