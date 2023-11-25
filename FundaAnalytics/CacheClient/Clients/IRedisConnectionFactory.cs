using StackExchange.Redis;

namespace CacheClient.Clients
{
    public interface IRedisConnectionFactory
    {
        ConnectionMultiplexer GetConnection();
    }
}