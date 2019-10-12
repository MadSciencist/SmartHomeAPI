using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Control
{
    public interface IControlCommand
    {
        Task Execute(JObject commandParams);
    }
}
