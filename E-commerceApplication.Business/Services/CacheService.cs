using E_commerceApplication.Business.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace E_commerceApplication.Business.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache) 
        {
            _memoryCache = memoryCache;
        }

        public T? Get<T>(string key) => _memoryCache
            .TryGetValue(key, out T? value) ? value : default;

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            MemoryCacheEntryOptions options = new()
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            _memoryCache
                .Set(key, value, options);
        }

        public void Remove(string key) => _memoryCache
            .Remove(key);
    }
}