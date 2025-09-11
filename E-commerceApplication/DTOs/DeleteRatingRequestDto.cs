using E_commerceApplication.Resources;
using System.ComponentModel.DataAnnotations;

namespace E_commerceApplication.DTOs
{
    public class DeleteRatingRequestDto
    {
        [MinLength(1, ErrorMessageResourceType = typeof(GamesDtoValidationMessages),
            ErrorMessageResourceName = nameof(GamesDtoValidationMessages.ListProductRequired))]
        public List<int> ProductIds { get; set; } = new();
    }
}