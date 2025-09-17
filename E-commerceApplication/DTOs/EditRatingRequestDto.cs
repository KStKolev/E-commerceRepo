using E_commerceApplication.Resources;
using System.ComponentModel.DataAnnotations;

namespace E_commerceApplication.DTOs
{
    public class EditRatingRequestDto
    {
        [Required(ErrorMessageResourceType = typeof(GamesDtoValidationMessages),
            ErrorMessageResourceName = nameof(GamesDtoValidationMessages.ProductIdRequired))]
        public int ProductId { get; set; }

        [Required(ErrorMessageResourceType = typeof(GamesDtoValidationMessages),
            ErrorMessageResourceName = nameof(GamesDtoValidationMessages.RatingRequired))]

        [Range(1,5, ErrorMessageResourceType = typeof(GamesDtoValidationMessages),
            ErrorMessageResourceName = nameof(GamesDtoValidationMessages.InvalidRating))]
        public int Rating { get; set; }
    }
}
