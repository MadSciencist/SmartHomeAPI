using Newtonsoft.Json.Linq;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.BusinessLogic
{
    public interface INodeService : IUserAuditable
    {
        Task<Node> CreateNode(string name, string identifier, string description, string type);
        Task<object> Control(int nodeId, string operations, JObject commandParams);
        IEnumerable<Command> GetNodeCommands(int nodeId);
    }
}