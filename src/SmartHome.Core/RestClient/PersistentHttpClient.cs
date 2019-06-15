using Microsoft.Extensions.Logging;
using RestSharp;
using SmartHome.Core.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.Core.RestClient
{
    public class PersistentHttpClient
    {
        private readonly ILogger _logger;
        private readonly IRestClient _client;
        private readonly RetryPolicy _retryPolicyProvider;

        public PersistentHttpClient(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(nameof(PersistentHttpClient));
            _retryPolicyProvider = new RetryPolicy(3);
            _client = CreateDefaultRestClient();
        }

        private IRestClient CreateDefaultRestClient()
        {
            var client = new RestSharp.RestClient();
            client.AddDefaultHeader("accept", "application/json");

            return client;
        }

        public async Task<object> InvokeAsync(string url, Method method)
        {
            return await InvokeAsync<object>(url, method);
        }

        public async Task<T> InvokeAsync<T>(string url, Method method) where T: class
        {
            _client.BaseUrl = new Uri(url); 
            var rr = new RestRequest("", method, DataFormat.Json);
            var result = await ExecuteRequestAsync<T>(rr);

            return result;
        }

        private async Task<T> ExecuteRequestAsync<T>(IRestRequest request) where T: class
        {
            try
            {
                var response = await _client.ExecuteWithPolicyAsync<T>(request, CancellationToken.None, _retryPolicyProvider.GetDefaultAsyncPolicy<T>());
                if (response != null && response.IsSuccessful)
                {
                    return response.Data;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw new SmartHomeException("Cannot connect to node");
            }

            return default(T);
        }
    }
}
