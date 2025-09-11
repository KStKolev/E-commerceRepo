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

        public async Task EditRatingProductAsync(Product product, ApplicationUser user, int rating)
        {
            ProductRating? productRating = await _context
                .ProductRatings
                .FirstOrDefaultAsync(pr => pr.ProductId == product.Id && pr.UserId == user.Id);

            if (productRating != null)
            {
                productRating.Rating = rating;
            }
            else
            {
                productRating = new ProductRating
                {
                    ProductId = product.Id,
                    Product = product,
                    UserId = user.Id,
                    User = user,
                    Rating = rating
                };

                await _context
                    .ProductRatings
                    .AddAsync(productRating);
            }

            product.TotalRating = product.Ratings
                .Sum(r => r.Rating);

            await _context
                .SaveChangesAsync();
        }

        public async Task DeleteRatingsAsync(ApplicationUser user, List<Product> products)
        {
            foreach (Product product in products)
            {
                ProductRating? productRating = _context.ProductRatings
                    .FirstOrDefault(pr => pr.ProductId == product.Id && pr.UserId == user.Id);

                if (productRating == null)
                {
                    continue;
                }

                user.Ratings
                    .Remove(productRating);

                product.Ratings
                    .Remove(productRating);

                product.TotalRating = product.Ratings
                    .Sum(r => r.Rating);

                productRating.IsDeleted = true;
            }

            await _context
                .SaveChangesAsync();
        }

        public async Task<Product?> GetProductWithRatingsAsync(int productId)
        {
            return await _context
                .Products
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<ApplicationUser?> GetUserWithRatingsAsync(Guid userId)
        {
            return await _context
                .Users
                .Include(u => u.Ratings)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}