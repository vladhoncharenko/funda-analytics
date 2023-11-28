namespace CacheClient.Services
{
    /// <summary>
    /// Service for interacting with a Redis cache using JSON commands.
    /// </summary>
    public interface IJsonCacheService
    {
        /// <summary>
        /// Sets JSON data in the Redis cache asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of data to be stored.</typeparam>
        /// <param name="key">The key under which the data should be stored in the cache.</param>
        /// <param name="path">The JSON path where the data should be stored within the key.</param>
        /// <param name="value">The value to be stored in the cache.</param>
        /// <returns>A Task that represents the asynchronous operation and contains a boolean indicating success.</returns>
        Task<bool> SetJsonDataAsync<T>(string key, string path, T value);

        /// <summary>
        /// Gets JSON data from the Redis cache asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of data to be retrieved.</typeparam>
        /// <param name="key">The key under which the data is stored in the cache.</param>
        /// <param name="path">The JSON path where the data is stored within the key.</param>
        /// <returns>A Task that represents the asynchronous operation and contains the retrieved data, or null if unsuccessful.</returns>
        Task<T?> GetJsonDataAsync<T>(string key, string path);
    }
}