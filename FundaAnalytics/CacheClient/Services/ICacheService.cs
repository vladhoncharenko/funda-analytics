namespace CacheClient.Services
{
    /// <summary>
    /// Service for interacting with a Redis cache using basic string-based commands.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Sets data in the Redis cache asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of data to be stored.</typeparam>
        /// <param name="key">The key under which the data should be stored in the cache.</param>
        /// <param name="value">The value to be stored in the cache.</param>
        /// <returns>A Task that represents the asynchronous operation and contains a boolean indicating success.</returns>
        Task<bool> SetDataAsync<T>(string key, T value);

        /// <summary>
        /// Gets data from the Redis cache asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of data to be retrieved.</typeparam>
        /// <param name="key">The key under which the data is stored in the cache.</param>
        /// <returns>A Task that represents the asynchronous operation and contains the retrieved data, or null if unsuccessful.</returns>
        Task<T?> GetDataAsync<T>(string key);

        /// <summary>
        /// Increments the value of a string key in the Redis cache asynchronously.
        /// </summary>
        /// <param name="key">The key whose value should be incremented.</param>
        /// <returns>A Task that represents the asynchronous operation and contains the incremented value.</returns>
        Task<long> StringDataIncrementAsync(string key);

        /// <summary>
        /// Sets an expiration time for a key in the Redis cache asynchronously.
        /// </summary>
        /// <param name="key">The key for which to set the expiration time.</param>
        /// <param name="timeSpan">The duration of time until the key expires.</param>
        /// <returns>A Task that represents the asynchronous operation and contains a boolean indicating success.</returns>
        Task<bool> DataKeyExpireAsync(string key, TimeSpan timeSpan);
    }
}