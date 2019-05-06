using SmartHome.Core.BusinessLogic;
using SmartHome.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SmartHome.Core.Control.Rest
{
    public class RestControlStrategy : IControlStrategy
    {
        private readonly PersistentHttpClient _httpClient;
        private readonly IRestTemplateBuilder _builder;

        public RestControlStrategy(PersistentHttpClient httpClient, IRestTemplateBuilder builder)
        {
            _httpClient = httpClient;
            _builder = builder;
        }

        public async Task<object> Execute(Node node, ControlCommand command)
        {
            switch (command.CommandType)
            {
                case EControlCommand.GetState:
                    return await _httpClient.GetAsync(GetUri(node, command));

                case EControlCommand.SetState:
                    return await _httpClient.GetAsync(GetUri(node, command));

                default:
                    throw new NotImplementedException($"Command {command.CommandType} is not implemented");
            }
        }

        private static string GetUri(Node node, ControlCommand command)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "apikey", "03102E55CD7BBE35"},
                { "value", "2"}
            };

            string baseUri = @"http://192.168.0.101:80";
            string cmd = "/api/relay/0";

            QueryStringBuilder builder = new QueryStringBuilder(baseUri + cmd);

            foreach (var queryParam in queryParams)
            {
                builder.Add(queryParam);
            }

            string uri = builder.ToString();

            return uri;
        }
    }
}