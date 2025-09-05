using E_commerceApplication.DAL.Entities;
using E_commerceApplication.Resources;
using System.ComponentModel.DataAnnotations;

namespace E_commerceApplication.DTOs
{
    public class GamesDto
    {
        [Required(ErrorMessageResourceType = typeof(GamesDtoValidationMessages),
            ErrorMessageResourceName = nameof(GamesDtoValidationMessages.NameRequired))]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(GamesDtoValidationMessages),
            ErrorMessageResourceName = nameof(GamesDtoValidationMessages.GenreRequired))]
        public string Genre { get; set; } = string.Empty;

        [EnumDataType(typeof(Platforms), ErrorMessageResourceType = typeof(GamesDtoValidationMessages),
            ErrorMessageResourceName = nameof(GamesDtoValidationMessages.PlatformRequired))]
        public Platforms Platform { get; set; }

        [Required(ErrorMessageResourceType = typeof(GamesDtoValidationMessages),
            ErrorMessageResourceName = nameof(GamesDtoValidationMessages.LogoRequired))]
        public IFormFile Logo { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(GamesDtoValidationMessages),
            ErrorMessageResourceName = nameof(GamesDtoValidationMessages.BackgroundRequired))]
        public IFormFile Background { get; set; } = null!;

        [EnumDataType(typeof(Rating), ErrorMessageResourceType = typeof(GamesDtoValidationMessages),
            ErrorMessageResourceName = nameof(GamesDtoValidationMessages.RatingRequired))]
        public Rating Rating { get; set; }

        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(GamesDtoValidationMessages),
            ErrorMessageResourceName = nameof(GamesDtoValidationMessages.CountRequired))]
        public int Count { get; set; }

        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(GamesDtoValidationMessages),
            ErrorMessageResourceName = nameof(GamesDtoValidationMessages.PriceRequired))]
        public decimal Price { get; set; }
    }
}