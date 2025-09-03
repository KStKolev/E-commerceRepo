using E_commerceApplication.DAL.Entities;

namespace E_commerceApplication.DAL.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> SearchProductsAsync(string term, int limit, int offset);
        Task<List<Platforms>> GetTopProductPlatformsAsync();
    }
}
