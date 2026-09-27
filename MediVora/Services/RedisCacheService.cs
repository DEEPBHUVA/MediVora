using MediVora.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace MediVora.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }


        public async Task<T?> GetDataAsync<T>(string key)
        {
            var data = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(data))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(data, _jsonOptions);
        }


        public async Task SetDataAsync<T>(string key, T data, TimeSpan expiration)
        {
            var serializedData = JsonSerializer.Serialize(data, _jsonOptions);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            await _cache.SetStringAsync(key, serializedData, cacheOptions);
        }


        public async Task RemoveDataAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
