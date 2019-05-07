using Newtonsoft.Json.Linq;
using SmartHome.Core.Control.Rest.Common;
using SmartHome.Core.Infrastructure;
using SmartHome.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Control.Rest.Implementations.Espurna
{
    public class SetRelay : IControlStrategy
    {
        private readonly PersistentHttpClient _httpClient;

        public SetRelay(PersistentHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<object> Execute(Node node, Command command, JObject commandParams)
        {
            var relayNo = commandParams.Value<string>("relayNo");
            var relayState = commandParams.Value<string>("state");

            if (string.IsNullOrEmpty(relayState)) throw new SmartHomeException("Relay state cannot be null: missing 'state' key");
            if (string.IsNullOrEmpty(relayNo)) throw new SmartHomeException("Relay number cannot be null: missing 'relayNo' key");
            if (string.IsNullOrEmpty(node.ApiKey)) throw new SmartHomeException("API key cannot be empty");

            var queryParams = new Dictionary<string, string>
                {
                    { "apikey", node.ApiKey },
                    { "value", relayState }
                };

            var baseUri = $"{node.IpAddress}:{node.Port}/api/relay/{relayNo}";
            var builder = new QueryStringBuilder(baseUri);

            foreach (var queryParam in queryParams)
                builder.Add(queryParam);

            var uri = builder.ToString();

            return await _httpClient.GetAsync(uri);
        }
    }
}
