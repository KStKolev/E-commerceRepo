using E_commerceApplication.Business.Models;

namespace E_commerceApplication.Business.Interfaces
{
    public interface IRatingService
    {
        Task<bool> EditRatingGameAsync(EditRatingModel editRatingModel);

        Task<bool> DeleteRatingsAsync(DeleteRatingModel deleteRatingModel);
    }
}
