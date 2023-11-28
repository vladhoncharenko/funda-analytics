using StackExchange.Redis;

namespace CacheClient.Clients
{
    /// <inheritdoc/>
    public class RedisConnectionFactory : IRedisConnectionFactory
    {
        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisConnectionFactory"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string for the Redis server.</param>
        public RedisConnectionFactory(string connectionString)
        {
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString));
        }

        /// <inheritdoc/>
        public ConnectionMultiplexer GetConnection() => _lazyConnection.Value;
    }
}