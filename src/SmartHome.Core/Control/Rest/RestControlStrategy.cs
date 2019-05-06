using Newtonsoft.Json;
using SmartHome.Core.BusinessLogic;
using SmartHome.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Control.Rest
{
    public class RestControlStrategy : IControlStrategy
    {
        private readonly PersistentHttpClient _httpClient;

        public RestControlStrategy(PersistentHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<object> Execute(Node node, NodeCommand command)
        {
            switch (command.Type)
            {
                case "GET":
                    return await _httpClient.GetAsync(GetUri(node, command));

                case "SET":
                    return await _httpClient.GetAsync(GetUri(node, command));

                default:
                    throw new NotImplementedException($"Command {JsonConvert.SerializeObject(command)} is not implemented");
            }
        }

        private static string GetUri(Node node, NodeCommand command)
        {
            var queryParams = new Dictionary<string, string>
            {
                // ReSharper disable once StringLiteralTypo
                { "apikey", node.ApiKey},
                { "value", command.Value}
            };

            var baseUri = $"{node.IpAddress}:{node.Port}";

            var builder = new QueryStringBuilder(baseUri + command.BaseUri);

            foreach (var queryParam in queryParams)
            {
                builder.Add(queryParam);
            }

            return builder.ToString();
        }
    }
}