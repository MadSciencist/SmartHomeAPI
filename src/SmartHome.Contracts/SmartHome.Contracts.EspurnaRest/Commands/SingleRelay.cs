using Autofac;
using Newtonsoft.Json.Linq;
using RestSharp;
using SmartHome.Core.Control;
using SmartHome.Core.Domain.ContractParams;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Infrastructure.Attributes;
using SmartHome.Core.MessageHanding;
using SmartHome.Core.RestClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Contracts.EspurnaRest.Commands
{
    [DisplayText("Single Relay")]
    public class SingleRelay : RestControlStrategyBase, IControlCommand
    {
        private const string RelayKey = "relay/0";

        public SingleRelay(ILifetimeScope container) : base(container)
        {
        }

        public async Task Execute(Node node, JObject commandParams)
        {
            var param = commandParams.ToObject<SingleRelayParam>();

            if (string.IsNullOrEmpty(param.State))
                throw new SmartHomeException("Relay state cannot be null: missing 'state' key");
            if (string.IsNullOrEmpty(param.RelayNo))
                throw new SmartHomeException("Relay number cannot be null: missing 'relayNo' key");
            if (string.IsNullOrEmpty(node.ApiKey)) throw new SmartHomeException("API key cannot be empty");

            var uri = BuildUri(node, param);

            var response = await HttpClient.InvokeAsync<Dictionary<string, string>>(uri, Method.GET);

            if (response != null)
            {
                INodeDataMapper mapper = base.GetNodeMapper(node);

                // Espurna response for SingleRelay has key relay/0
                var property = mapper?.GetPhysicalPropertyByContractMagnitude(RelayKey);

                // Check if there is associated system value
                if (property != null)
                {
                    var value = response[RelayKey];
                    NotificationService.PushNodeDataNotification(node.Id, property, value);
                    await NodeDataService.AddSingleAsync(node.Id, EDataRequestReason.Node,
                        new NodeDataMagnitudeDto(property, value));
                }
            }
        }

        private static string BuildUri(Node node, SingleRelayParam param)
        {
            var queryParams = new Dictionary<string, string>
            {
                {"apikey", node.ApiKey},
                {"value", param.State}
            };

            var baseUri = $"{node.IpAddress}:{node.Port}/api/relay/{param.RelayNo}";
            var builder = new QueryStringBuilder(baseUri);

            foreach (var queryParam in queryParams)
                builder.Add(queryParam);

            return builder.ToString();
        }
    }
}
