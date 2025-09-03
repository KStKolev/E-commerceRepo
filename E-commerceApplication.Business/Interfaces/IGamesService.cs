using E_commerceApplication.DAL.Entities;

namespace E_commerceApplication.Business.Interfaces
{
    public interface IGamesService
    {
        Task<List<Platforms>> GetTopGamePlatformsAsync();
        Task<List<Product>> GetSearchedGamesAsync(string term, int limit, int offset);
    }
}
