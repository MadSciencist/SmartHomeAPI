using SmartHome.Core.BusinessLogic;
using SmartHome.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

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
                    return await _httpClient.GetAsync(@"http://192.168.0.100/api/relay/0?apikey=D8F3A6CC12FE9CC9");

                case EControlCommand.SetState:
                    return await _httpClient.PutAsync(@"http://192.168.0.101/api/relay/0", GetBody(node, command));

                default:
                    throw new NotImplementedException($"Command {command.CommandType} is not implemented");
            }
        }

        // TODO move to builder
        private static HttpContent GetBody(Node node, ControlCommand command)
        {
            var dict = new Dictionary<string, string>
            {
                { "apiKey", "03102E55CD7BBE35"},
                { "value", "2"}
            };
            //dict.Add("apikey", node.ApiKey);

            HttpContent body = new FormUrlEncodedContent(dict);
            return body;
        }
    }
}