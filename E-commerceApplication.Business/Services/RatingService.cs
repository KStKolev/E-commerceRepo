using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Models;
using E_commerceApplication.DAL.Entities;
using E_commerceApplication.DAL.Interfaces;

namespace E_commerceApplication.Business.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<bool> EditRatingGameAsync(EditRatingModel editRatingModel)
        {
            Product? product = await _ratingRepository
                .GetProductWithRatingsAsync(editRatingModel.ProductId);

            ApplicationUser? user = await _ratingRepository
                .GetUserWithRatingsAsync(editRatingModel.UserId);

            if (product == null || user == null)
            {
                return false;
            }

            await _ratingRepository
                .EditRatingProductAsync(product, user, editRatingModel.Rating);

            return true;
        }

        public async Task<bool> DeleteRatingsAsync(DeleteRatingModel deleteRatingModel)
        {
            ApplicationUser? user = await _ratingRepository
                .GetUserWithRatingsAsync(deleteRatingModel.UserId);

            if (user == null)
            {
                return false;
            }

            List<Product> products = new ();

            foreach (int productId in deleteRatingModel.ProductIds)
            {
                Product? product = await _ratingRepository
                    .GetProductWithRatingsAsync(productId);

                if (product == null)
                {
                    return false;
                }

                products.Add(product);
            }

            await _ratingRepository
                .DeleteRatingsAsync(user, products);

            return true;
        }
    }
}