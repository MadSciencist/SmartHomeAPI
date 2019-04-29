using SmartHome.Core.BusinessLogic;
using SmartHome.Domain.Entity;
using System;
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
            object response;

            switch (command.CommandType)
            {
                case EControlCommand.GetState:
                    response = await _httpClient.GetAsync(@"http://192.168.0.100/api/relay/0?apikey=D8F3A6CC12FE9CC9");
                    break;

                default: throw new NotImplementedException($"Command {command.CommandType} is not implemented");
            }

            return response;
        }
    }
}