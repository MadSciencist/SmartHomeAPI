using Newtonsoft.Json.Linq;
using SmartHome.Core.Control;
using SmartHome.Core.Domain.ContractParams;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Infrastructure.Attributes;
using System;
using System.Threading.Tasks;

namespace SmartHome.Contracts.GenericRest.Commands
{
    [ControlContract(ContractType.Rest)]
    [DisplayText("Single Relay")]
    public class SingleRelay : IControlStrategy
    {
        public Task Execute(Node node, JObject commandParams)
        {
            var param = commandParams.ToObject<SingleRelayParam>();

            if (string.IsNullOrEmpty(param.State)) throw new SmartHomeException("Relay state cannot be null: missing 'state' key");
            if (string.IsNullOrEmpty(param.RelayNo)) throw new SmartHomeException("Relay number cannot be null: missing 'relayNo' key");
            if (string.IsNullOrEmpty(node.ApiKey)) throw new SmartHomeException("API key cannot be empty");

            throw new NotImplementedException();
        }
    }
}
