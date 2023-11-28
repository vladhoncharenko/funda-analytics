using StackExchange.Redis;

namespace CacheClient.Wrappers
{
    /// <inheritdoc />
    public class ConnectionMultiplexerWrapper : IConnectionMultiplexerWrapper
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public ConnectionMultiplexerWrapper(string connectionString)
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
        }

        /// <inheritdoc />
        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.GetDatabase();
        }
    }
}
