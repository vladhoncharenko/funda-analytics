using CacheClient.Clients;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using System.Text.Json;

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
            return _jsonCacheCommands.SetAsync(key, path, JsonSerializer.Serialize(value));
        }

        public Task<T?> GetDataAsync<T>(string key, string path)
        {
            return _jsonCacheCommands.GetAsync<T>(key, path);
        }
    }
}