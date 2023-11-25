using StackExchange.Redis;

namespace CacheClient.Clients
{
    public class RedisConnectionFactory : IRedisConnectionFactory
    {
        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;

        public RedisConnectionFactory(string connectionString)
        {
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString));
        }

        public ConnectionMultiplexer GetConnection() => _lazyConnection.Value;
    }
}