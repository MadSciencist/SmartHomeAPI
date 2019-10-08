using Newtonsoft.Json.Linq;
using SmartHome.Core.Entities.Entity;
using System.Threading.Tasks;

namespace SmartHome.Core.Control
{
    public interface IControlCommand
    {
        Task Execute(Node node, JObject commandParams);
    }
}
