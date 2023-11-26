namespace CacheClient.Services
{
    public interface ICacheService
    {
        Task<bool> SetDataAsync<T>(string key, T value);

        Task<T?> GetDataAsync<T>(string key);

        Task<long> StringDataIncrementAsync(string key);

        Task<bool> DataKeyExpireAsync(string key, TimeSpan timeSpan);
    }
}