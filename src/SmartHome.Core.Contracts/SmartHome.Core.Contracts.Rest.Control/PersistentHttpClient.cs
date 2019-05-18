using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SmartHome.Core.Infrastructure;

namespace SmartHome.Core.Contracts.Rest.Control
{
    public class PersistentHttpClient : IDisposable
    {
        private static HttpClient _httpClient;

        public PersistentHttpClient()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetAsync(string url)
        {
            //TODO move to http client factory
            //    Random jitterer = new Random();
            //    var polly = Policy
            //        .Handle<HttpRequestException>()
            //        .WaitAndRetryAsync(2, // exponential back-off plus some jitter
            //            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            //                            + TimeSpan.FromMilliseconds(jitterer.Next(0, 2))
            //            , (ex, timeSpan, retryCount, context) =>
            //            {
            //                Console.WriteLine($"msg: {ex.Message} retry: {retryCount}");
            //            });

            //HttpResponseMessage response = await polly.ExecuteAsync(() => _httpClient.GetAsync(url));

            //    if (response.IsSuccessStatusCode)
            //        return await response.Content.ReadAsStringAsync();

            //    return string.Empty;


            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            throw new SmartHomeException("Sensor malfunction");
        }

        public async Task<string> PutAsync(string url, HttpContent body)
        {
            var response = await _httpClient.PutAsync(url, body);
            var asd = await body.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            throw new SmartHomeException("Sensor malfunction");
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
