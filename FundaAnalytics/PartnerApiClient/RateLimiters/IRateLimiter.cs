namespace PartnerApiClient.RateLimiters
{
    public interface IRateLimiter
    {
        Task<bool> ShouldLimitRequestAsync(string key);
    }
}