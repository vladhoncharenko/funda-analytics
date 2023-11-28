using StackExchange.Redis;

namespace CacheClient.Wrappers
{
    /// <summary>
    /// This class is a wrapper around the ConnectionMultiplexer class from StackExchange.Redis.
    /// It allows for easier testing and dependency injection.
    /// </summary>
    public interface IConnectionMultiplexerWrapper
    {
        /// <summary>
        /// Gets the database from the Redis server.
        /// </summary>
        /// <returns>The IDatabase instance representing the Redis database.</returns>
        IDatabase GetDatabase();
    }
}