using CacheClient.Clients;
using StackExchange.Redis;
using System.Text.Json;

namespace CacheClient.Services
{
    /// <inheritdoc/>
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _database;

        public RedisCacheService(IRedisConnectionFactory redisConnectionFactory)
        {
            _database = redisConnectionFactory.GetConnection().GetDatabase();
        }

        /// <inheritdoc/>
        public async Task<bool> SetDataAsync<T>(string key, T value)
        {
            return await _database.StringSetAsync(key, JsonSerializer.Serialize(value));
        }

        /// <inheritdoc/>
        public async Task<T?> GetDataAsync<T>(string key)
        {
            var redisValue = await _database.StringGetAsync(key);

            return redisValue.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(redisValue);
        }

        /// <inheritdoc/>
        public async Task<long> StringDataIncrementAsync(string key)
        {
            return await _database.StringIncrementAsync(key);
        }

        /// <inheritdoc/>
        public async Task<bool> DataKeyExpireAsync(string key, TimeSpan timeSpan)
        {
            return await _database.KeyExpireAsync(key, timeSpan);
        }
    }
}