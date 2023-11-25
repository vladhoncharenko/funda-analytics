namespace CacheClient.Services
{
    public interface ICacheService
    {
        Task<bool> SetDataAsync<T>(string key, string path, T value);

        Task<T?> GetDataAsync<T>(string key, string path);
    }
}