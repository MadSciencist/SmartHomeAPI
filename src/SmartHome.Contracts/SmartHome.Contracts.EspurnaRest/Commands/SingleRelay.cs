using Autofac;
using Newtonsoft.Json.Linq;
using RestSharp;
using SmartHome.Core.Control;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Attributes;
using SmartHome.Core.Entities.ContractParams;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Infrastructure.Exceptions;
using SmartHome.Core.MessageHanding;
using SmartHome.Core.RestClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Contracts.EspurnaRest.Commands
{
    [DisplayText("Single Relay")]
    [ParameterType(typeof(SingleRelayParam))]
    public class SingleRelay : RestControlCommand, IControlCommand
    {
        private const string RelayKey = "relay/0";

        public SingleRelay(ILifetimeScope container, Node node) : base(container, node)
        {
        }

        public async Task Execute(JObject commandParams)
        {
            var param = commandParams.ToObject<SingleRelayParam>().Validate();

            if (string.IsNullOrEmpty(Node.ApiKey)) throw new SmartHomeException("API key cannot be empty");

            var uri = BuildUri(Node, param);

            var response = await HttpClient.InvokeAsync<Dictionary<string, string>>(uri, Method.GET);

            if (response != null)
            {
                INodeDataMapper mapper = base.GetNodeMapper(Node);

                // Espurna response for SingleRelay has key relay/0
                var property = mapper?.GetPhysicalPropertyByContractMagnitude(RelayKey);

                // Check if there is associated system value
                if (property != null)
                {
                    var value = response[RelayKey];
                    var magnitudeDto = new NodeDataMagnitudeDto(property, value);
                    NotificationService.PushDataNotification(Node.Id, magnitudeDto);
                    await NodeDataService.AddSingleAsync(Node.Id, magnitudeDto);
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
