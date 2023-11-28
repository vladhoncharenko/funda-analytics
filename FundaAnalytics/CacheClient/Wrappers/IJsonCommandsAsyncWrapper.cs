using CacheClient.Clients;
using NRedisStack;

namespace CacheClient.Wrappers
{
    /// <summary>
    /// This class serves as a wrapper for asynchronous JSON-related Redis commands, providing a convenient interface
    /// for interacting with a Redis database using the NRedisStack library.
    /// </summary>
    public interface IJsonCommandsAsyncWrapper
    {
        /// <summary>
        /// Gets an instance of the IJsonCommandsAsync interface for executing asynchronous JSON-related commands on a Redis database.
        /// </summary>
        /// <param name="redisConnectionFactory"></param>
        /// <returns></returns>
        IJsonCommandsAsync GetJsonCommandsAsync(IRedisConnectionFactory redisConnectionFactory);
    }
}