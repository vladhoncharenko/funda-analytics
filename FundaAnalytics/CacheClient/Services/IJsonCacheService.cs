namespace CacheClient.Services
{
    public interface IJsonCacheService
    {
        Task<bool> SetJsonDataAsync<T>(string key, string path, T value);

        Task<T?> GetJsonDataAsync<T>(string key, string path);
    }
}