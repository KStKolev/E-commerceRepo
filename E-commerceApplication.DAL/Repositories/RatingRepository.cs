using E_commerceApplication.DAL.Data;
using E_commerceApplication.DAL.Entities;
using E_commerceApplication.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerceApplication.DAL.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly ApplicationDbContext _context;

        public RatingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task EditRatingProductAsync(int productId, Guid userId, int rating)
        {
            ProductRating? productRating = await _context
                .ProductRatings
                .FirstOrDefaultAsync(pr => pr.ProductId == productId && pr.UserId == userId);

            if (productRating != null)
            {
                productRating.Rating = rating;
            }
            else
            {
                productRating = new ProductRating
                {
                    ProductId = productId,
                    UserId = userId,
                    Rating = rating
                };

                await _context
                    .ProductRatings
                    .AddAsync(productRating);
            }

            await _context
                .SaveChangesAsync();
        }

        public async Task DeleteRatingsAsync(Guid userId, List<int> productIdList)
        {
            HashSet<int> productIdSet = new(productIdList);

            List<ProductRating> productRatingList = await _context
                .ProductRatings
                .Where(pr => pr.UserId == userId && productIdSet.Contains(pr.ProductId))
                .ToListAsync();

            _context
                .ProductRatings
                .RemoveRange(productRatingList);

            await _context
                .SaveChangesAsync();
        }
    }
}