namespace E_commerceApplication.DAL.Interfaces
{
    public interface IRatingRepository
    {
        Task EditRatingProductAsync(int productId, Guid userId, int rating);

        Task DeleteRatingsAsync(Guid userId, List<int> productIdsList);

        Task<bool> CheckProductWithIdAsync(int productId);
    }
}
