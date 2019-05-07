using SmartHome.Domain.Entity;
using System;
using System.Threading.Tasks;

namespace SmartHome.Core.Control.Rest.Implementations.Espurna
{
    public class ToggleRelay : IControlStrategy
    {
        public Task<object> Execute(Node node, Command command)
        {
            throw new NotImplementedException();
        }
    }
}
