using E_commerceApplication.DAL.Entities;

namespace E_commerceApplication.DAL.Interfaces
{
    public interface IRatingRepository
    {
        Task EditRatingProductAsync(Product product, ApplicationUser user, int rating);

        Task DeleteRatingsAsync(ApplicationUser user, List<Product> products);

        Task<Product?> GetProductWithRatingsAsync(int productId);

        Task<ApplicationUser?> GetUserWithRatingsAsync(Guid userId);
    }
}
