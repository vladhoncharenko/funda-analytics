using CacheClient.Clients;
using Newtonsoft.Json;
using NRedisStack;
using NRedisStack.RedisStackCommands;

namespace CacheClient.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly JsonCommands _jsonCacheCommands;

        public RedisCacheService(IRedisConnectionFactory redisConnectionFactory)
        {
            _jsonCacheCommands = redisConnectionFactory.GetConnection().GetDatabase().JSON(); ;
        }

        public Task<bool> SetDataAsync<T>(string key, string path, T value)
        {
            return _jsonCacheCommands.SetAsync(key, path, JsonConvert.SerializeObject(value));
        }

        public Task<T?> GetDataAsync<T>(string key, string path)
        {
            return _jsonCacheCommands.GetAsync<T>(key, path);
        }
    }
}