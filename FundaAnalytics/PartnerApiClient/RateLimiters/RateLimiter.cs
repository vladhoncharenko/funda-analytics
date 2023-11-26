using CacheClient.Services;

namespace PartnerApiClient.RateLimiters
{
    public class RateLimiter : IRateLimiter
    {
        private readonly ICacheService _cacheService;
        private readonly int _rateLimitPerMinute;

        public RateLimiter(ICacheService cacheService)
        {
            _cacheService = cacheService;
            _rateLimitPerMinute = Convert.ToInt32(Environment.GetEnvironmentVariable("RateLimitPerMinute"));
        }

        public async Task<bool> ShouldLimitRequestAsync(string key)
        {
            var value = await _cacheService.GetDataAsync<int?>(key);
            if (value >= _rateLimitPerMinute)
                return true;

            await _cacheService.StringDataIncrementAsync(key);
            await _cacheService.DataKeyExpireAsync(key, TimeSpan.FromMinutes(1));

            return false;
        }
    }
}
