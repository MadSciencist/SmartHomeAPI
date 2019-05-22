using Polly;
using Polly.Retry;
using RestSharp;
using System.Net;

namespace SmartHome.Core.RestClient
{
    public class RetryPolicy
    { 
        public int RetryCount { get; set; }

        public RetryPolicy(int retryCount)
        {
            RetryCount = retryCount;
        }

        public AsyncRetryPolicy<IRestResponse<T>> GetDefaultAsyncPolicy<T>()
        {
            return Policy.HandleResult<IRestResponse<T>>(ResultPredicate)
                .RetryAsync(RetryCount);
        }

        private static bool ResultPredicate(IRestResponse response)
        {
            var isCompleted = response.ResponseStatus == ResponseStatus.Completed;
            var isNotOk =  response.StatusCode != HttpStatusCode.OK;

            return isCompleted && isNotOk;
        }
    }
}
