using E_commerceApplication.DAL.Entities;

namespace E_commerceApplication.DAL.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> SearchProductsAsync(string term, int limit, int offset);

        Task<List<Platforms>> GetTopProductPlatformsAsync();

        Task<Product?> GetProductByIdAsync(int productId);

        Task<Product> CreateProductAsync(Product product);

        Task UpdateProductAsync(Product product);

        Task DeleteProductAsync(Product product);
    }
}
