using E_commerceApplication.Business.Constants;
using E_commerceApplication.Business.Interfaces;

namespace E_commerceApplication.Business.Services
{
    public class UserCacheService : IUserCacheService
    {
        private ICacheService _cacheService;

        public UserCacheService(ICacheService cacheService) 
        {
            _cacheService = cacheService;
        }

        private string GetCacheKey(string userId) => $"{CacheKeyPrefixes.USER_CACHE_PREFIX}{userId}";

        public T? Get<T>(string userId)
        {
            return _cacheService
                .Get<T>(GetCacheKey(userId));
        }

        public void Remove(string userId)
        {
            _cacheService
                .Remove(GetCacheKey(userId));
        }

        public void Set<T>(string userId, T value, TimeSpan? absoluteExpiration = null)
        {
            int defaultExpiration = 30;

            _cacheService
                .Set(GetCacheKey(userId), value, absoluteExpiration ?? TimeSpan.FromMinutes(defaultExpiration));
        }
    }
}