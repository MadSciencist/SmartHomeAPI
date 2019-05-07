using Newtonsoft.Json.Linq;
using SmartHome.Domain.Entity;
using System.Threading.Tasks;

namespace SmartHome.Core.Control
{
    public interface IControlStrategy
    {
        Task<object> Execute(Node node, Command command, JObject commandParams);
    }
}
