using Newtonsoft.Json.Linq;
using SmartHome.Core.Control;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Infrastructure.Attributes;
using System;
using System.Threading.Tasks;

namespace SmartHome.Contracts.GenericRest.Commands
{
    [DisplayText("Third command")]
    public class ThirdCommand : IControlCommand
    {
        public Task Execute(Node node, JObject commandParams)
        {
            throw new NotImplementedException();
        }
    }
}
