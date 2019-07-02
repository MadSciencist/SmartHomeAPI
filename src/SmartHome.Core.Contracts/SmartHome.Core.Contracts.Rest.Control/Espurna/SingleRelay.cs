﻿using Newtonsoft.Json.Linq;
using RestSharp;
using SmartHome.Core.Contracts.Mappings;
using SmartHome.Core.Domain;
using SmartHome.Core.Domain.ContractParams;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Domain.Models;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.RestClient;
using SmartHome.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Contracts.Rest.Control.Espurna
{
    public class SingleRelay : IRestControlStrategy
    {
        private readonly PersistentHttpClient _httpClient;
        private readonly NotificationService _notificationService;
        private readonly INodeDataService _nodeDataService;
        private const string RelayKey = "relay/0";

        public SingleRelay(PersistentHttpClient httpClient, NotificationService notificationService, INodeDataService nodeDataService)
        {
            _httpClient = httpClient;
            _notificationService = notificationService;
            _nodeDataService = nodeDataService;
        }

        public async Task Execute(Node node, JObject commandParams)
        {
            var param = commandParams.ToObject<SingleRelayParam>();

            if (string.IsNullOrEmpty(param.State)) throw new SmartHomeException("Relay state cannot be null: missing 'state' key");
            if (string.IsNullOrEmpty(param.RelayNo)) throw new SmartHomeException("Relay number cannot be null: missing 'relayNo' key");
            if (string.IsNullOrEmpty(node.ApiKey)) throw new SmartHomeException("API key cannot be empty");

            var uri = BuildUri(node, param);

            var response = await _httpClient.InvokeAsync<Dictionary<string, string>>(uri, Method.GET);

            if (response != null)
            {
                // Espurna response for SingleRelay has key relay/0
                PhysicalProperty property = SystemMagnitudes.GetPhysicalPropertyByContextDictionary(EspurnaMapping.Map, RelayKey);

                // Check if there is associated system value
                if (property != null)
                {
                    var value = response[RelayKey];
                    _notificationService.PushNodeDataNotification(node.Id, property, value);
                    await _nodeDataService.AddSingleAsync(node.Id, EDataRequestReason.Node, new NodeDataMagnitudeDto(property, value));
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
