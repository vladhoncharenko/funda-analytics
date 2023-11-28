using CacheClient.Clients;
using NRedisStack;
using NRedisStack.RedisStackCommands;

namespace CacheClient.Wrappers
{
    /// <inheritdoc/>
    public class JsonCommandsAsyncWrapper : IJsonCommandsAsyncWrapper
    {
        /// <inheritdoc/>
        public IJsonCommandsAsync GetJsonCommandsAsync(IRedisConnectionFactory redisConnectionFactory)
        {
            return redisConnectionFactory.GetConnection().GetDatabase().JSON();
        }
    }
}