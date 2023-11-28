using CacheClient.Clients;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using System.Text.Json;

namespace CacheClient.Services
{
    /// <inheritdoc/>
    public class RedisJsonCacheService : IJsonCacheService
    {
        private readonly JsonCommands _jsonCacheCommands;

        public RedisJsonCacheService(IRedisConnectionFactory redisConnectionFactory)
        {
            _jsonCacheCommands = redisConnectionFactory.GetConnection().GetDatabase().JSON();
        }

        /// <inheritdoc/>
        public Task<bool> SetJsonDataAsync<T>(string key, string path, T value)
        {
            return _jsonCacheCommands.SetAsync(key, path, JsonSerializer.Serialize(value));
        }

        /// <inheritdoc/>
        public Task<T?> GetJsonDataAsync<T>(string key, string path)
        {
            return _jsonCacheCommands.GetAsync<T>(key, path);
        }
    }
}