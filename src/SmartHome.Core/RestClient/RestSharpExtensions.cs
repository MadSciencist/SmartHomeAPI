using Polly;
using Polly.Retry;
using RestSharp;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.Core.RestClient
{
    public static class RestSharpExtensions
    {
        public static IRestResponse ExecuteWithPolicy(this IRestClient client, IRestRequest request, Policy<IRestResponse> policy)
        {
            // capture the exception so we can push it though the standard response flow.
            var policyResult = policy.ExecuteAndCapture(() => client.Execute(request));

            return policyResult.Outcome == OutcomeType.Successful ? policyResult.Result : new RestResponse
            {
                Request = request,
                ErrorException = policyResult.FinalException
            };
        }

        public static async Task<IRestResponse> ExecuteWithPolicyAsync(
            this IRestClient client, 
            IRestRequest request, 
            CancellationToken cancellationToken, 
            AsyncRetryPolicy<IRestResponse> policy)
        {
            var policyResult = await policy.ExecuteAndCaptureAsync(ct => client.ExecuteTaskAsync(request, ct), cancellationToken);

            return policyResult.Outcome == OutcomeType.Successful ? policyResult.Result : new RestResponse
            {
                Request = request,
                ErrorException = policyResult.FinalException
            };
        }

        public static async Task<IRestResponse<T>> ExecuteWithPolicyAsync<T>(
            this IRestClient client, 
            IRestRequest request, 
            CancellationToken cancellationToken, 
            AsyncRetryPolicy<IRestResponse<T>> policy)
        {
            var policyResult = await policy.ExecuteAndCaptureAsync(ct => client.ExecuteTaskAsync<T>(request, ct), cancellationToken);

            return policyResult.Outcome == OutcomeType.Successful ? policyResult.Result : new RestResponse<T>
            {
                Request = request,
                ErrorException = policyResult.FinalException
            };
        }
    }
}
