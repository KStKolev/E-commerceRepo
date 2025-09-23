namespace E_commerceApplication.Business.Interfaces
{
    public interface IUserCacheService
    {
        T? Get<T>(string userId);

        void Set<T>(string userId, T value, TimeSpan? absoluteExpiration = null);

        void Remove(string userId);
    }
}