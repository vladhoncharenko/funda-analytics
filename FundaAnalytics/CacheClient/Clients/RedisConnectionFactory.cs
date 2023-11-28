using CacheClient.Wrappers;

namespace CacheClient.Clients
{
    /// <inheritdoc/>
    public class RedisConnectionFactory : IRedisConnectionFactory
    {
        private readonly Lazy<IConnectionMultiplexerWrapper> _lazyConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisConnectionFactory"/> class.
        /// </summary>
        public RedisConnectionFactory(IConnectionMultiplexerWrapper connectionMultiplexerWrapper)
        {
            _lazyConnection = new Lazy<IConnectionMultiplexerWrapper>(() => connectionMultiplexerWrapper);
        }

        /// <inheritdoc/>
        public IConnectionMultiplexerWrapper GetConnection() => _lazyConnection.Value;
    }
}