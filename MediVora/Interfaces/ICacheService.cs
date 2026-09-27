namespace MediVora.Interfaces
{
    public interface ICacheService
    {
        Task<T?> GetDataAsync<T>(string key);
        Task SetDataAsync<T>(string key, T data, TimeSpan expiration);
        Task RemoveDataAsync(string key);
    }
}
