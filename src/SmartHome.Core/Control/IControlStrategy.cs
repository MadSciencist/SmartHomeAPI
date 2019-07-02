using Newtonsoft.Json.Linq;
using SmartHome.Core.Domain.Entity;
using System.Threading.Tasks;

namespace SmartHome.Core.Control
{
    public interface IControlStrategy
    {
        Task Execute(Node node, JObject commandParams);
    }
}
