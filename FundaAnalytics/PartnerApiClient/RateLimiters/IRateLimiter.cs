namespace PartnerApiClient.RateLimiters
{
    /// <summary>
    /// Represents a rate limiter for controlling the API request rate.
    /// </summary> 
    public interface IRateLimiter
    {
        /// <summary>
        /// Checks whether a request with the specified key should be limited based on the configured rate limit.
        /// </summary>
        /// <param name="key">The key identifying the request type.</param>
        /// <returns>True if the request should be limited, otherwise false.</returns>
        Task<bool> ShouldLimitRequestAsync(string key);

        /// <summary>
        /// Waits until the rate limit is over.
        /// </summary>
        /// <param name="delay"></param>
        void WaitTillLimitEnd(int delay = 60000);
    }
}