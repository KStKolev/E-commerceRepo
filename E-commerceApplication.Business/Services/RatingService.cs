using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Models;
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
            if (!await _ratingRepository.CheckProductWithIdAsync(editRatingModel.ProductId))
            {
                return false;
            }

            await _ratingRepository
                .EditRatingProductAsync(editRatingModel.ProductId, editRatingModel.UserId, editRatingModel.Rating);

            return true;
        }

        public async Task DeleteRatingsAsync(DeleteRatingModel deleteRatingModel)
        {
            await _ratingRepository
                .DeleteRatingsAsync(deleteRatingModel.UserId, deleteRatingModel.ProductIds);
        }
    }
}