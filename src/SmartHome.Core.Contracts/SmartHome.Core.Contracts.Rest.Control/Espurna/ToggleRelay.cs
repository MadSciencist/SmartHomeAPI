using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.RestClient;

namespace SmartHome.Core.Contracts.Rest.Control.Espurna
{
    public class ToggleRelay : IRestControlStrategy
    {
        private readonly PersistentHttpClient _httpClient;

        public ToggleRelay(PersistentHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Example commandParams: 
        /// {
        //	    "relayNo": 0,
        //  }
        /// </summary>
        /// <param name="node">Target node</param>
        /// <param name="command">Control command</param>
        /// <param name="commandParams">Params from request body</param>
        /// <returns>RAW espurna response</returns>
        public async Task<object> Execute(Node node, JObject commandParams)
        {
            var relayNo = commandParams.Value<string>("relayNo");

            if (string.IsNullOrEmpty(relayNo)) throw new SmartHomeException("Relay number cannot be null: missing 'relayNo' key");
            if (string.IsNullOrEmpty(node.ApiKey)) throw new SmartHomeException("API key cannot be empty");

            const string toggleValue = "2";

            var queryParams = new Dictionary<string, string>
                {
                    { "apikey", node.ApiKey },
                    { "value", toggleValue }
                };

            var baseUri = $"{node.IpAddress}:{node.Port}/api/relay/{relayNo}";
            var builder = new QueryStringBuilder(baseUri);

            foreach (var queryParam in queryParams)
                builder.Add(queryParam);

            var uri = builder.ToString();

            var nodeResponse =  await _httpClient.InvokeAsync<Dictionary<string, string>>(uri, Method.GET);
            return nodeResponse;
        }
    }
}
