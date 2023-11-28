using Polly;
using Polly.Extensions.Http;

namespace PartnerApiClient.Utils
{
    /// <summary>
    /// Partner API retry policy.
    /// Retry Conditions:
    /// Transient HTTP Errors: Retries the request in the event of transient HTTP errors (status codes in the 5xx range).
    /// Unauthorized Status Code: Retries the request if the HTTP response has an Unauthorized (401) status code.
    /// 
    /// Retry Configuration:
    /// Maximum Retries: The method is configured to retry up to 6 times.
    /// Exponential Back off Strategy: The delay between retries follows an exponential back off strategy, increasing with each retry attempt.
    /// The delay is calculated as 2^retryAttempt seconds.
    /// </summary>
    public static class PartnerApiRetryPolicy
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}