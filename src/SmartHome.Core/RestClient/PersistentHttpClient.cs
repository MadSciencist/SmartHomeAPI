using Microsoft.Extensions.Logging;
using Polly.Retry;
using RestSharp;
using SmartHome.Core.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.Core.RestClient
{
    public class PersistentHttpClient
    {
        private readonly IRestClient _client;
        private readonly RetryPolicy _retryPolicyProvider;
        private readonly ILogger _logger;

        public PersistentHttpClient(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(nameof(PersistentHttpClient));
            _client = new RestSharp.RestClient();
            _client.AddDefaultHeader("accept", "application/json");

            _retryPolicyProvider = new RetryPolicy(3);
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

        public async Task<T> ExecuteRequestAsync<T>(IRestRequest request) where T: class
        {
            IRestResponse<T> response; 

            try
            {
                response = await _client.ExecuteWithPolicyAsync<T>(request, CancellationToken.None, _retryPolicyProvider.GetDefaultAsyncPolicy<T>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                throw new SmartHomeException("Sensor malfunction");
            }

            if (response != null && response.IsSuccessful)
            {
                return response.Data;
            }

            return default(T);
        }
    }
}
