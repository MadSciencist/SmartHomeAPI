using System.Net;
using Polly;
using Polly.Retry;
using RestSharp;

namespace SmartHome.Core.RestClient
{
    public class RetryPolicy
    {
        public AsyncRetryPolicy<IRestResponse> AsyncPolicy { get; set; }
        private readonly int _retryCount;

        public RetryPolicy(int retryCount)
        {
            _retryCount = retryCount;
        }

        public AsyncRetryPolicy<IRestResponse> GetDefaultAsyncPolicy()
        {
            AsyncPolicy = Polly.Policy.HandleResult<IRestResponse>(ResultPredicate)
                .Or<WebException>()
                .RetryAsync(_retryCount);

            return AsyncPolicy;
        }

        public AsyncRetryPolicy<IRestResponse<T>> GetDefaultAsyncPolicy<T>()
        {
            return Polly.Policy.HandleResult<IRestResponse<T>>(ResultPredicate)
                .Or<WebException>()
                .RetryAsync(_retryCount);
        }


        private bool ResultPredicate(IRestResponse response)
        {
            return response.StatusCode != HttpStatusCode.OK;
        }
    }
}
