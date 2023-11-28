using StackExchange.Redis;

namespace CacheClient.Clients
{
    /// <summary>
    /// Factory for creating and managing connections to a Redis server.
    /// </summary>
    public interface IRedisConnectionFactory
    {
        /// <summary>
        /// Gets the connection to the Redis server.
        /// </summary>
        /// <returns>The ConnectionMultiplexer instance.</returns>
        ConnectionMultiplexer GetConnection();
    }
}