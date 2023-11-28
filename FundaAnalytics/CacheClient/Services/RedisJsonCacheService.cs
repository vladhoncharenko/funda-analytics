using CacheClient.Clients;
using CacheClient.Wrappers;
using NRedisStack;
using System.Text.Json;

namespace CacheClient.Services
{
    /// <inheritdoc/>
    public class RedisJsonCacheService : IJsonCacheService
    {
        private readonly IJsonCommandsAsync _jsonCacheCommands;

        public RedisJsonCacheService(IRedisConnectionFactory redisConnectionFactory, IJsonCommandsAsyncWrapper jsonCommandsAsyncWrapper)
        {
            _jsonCacheCommands = jsonCommandsAsyncWrapper.GetJsonCommandsAsync(redisConnectionFactory);
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