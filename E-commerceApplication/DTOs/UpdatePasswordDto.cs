using E_commerceApplication.Resources;
using E_commerceApplication.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_commerceApplication.DTOs
{
    public class UpdatePasswordDto
    {
        [Required(ErrorMessageResourceType = typeof(AuthDtoValidationMessages),
            ErrorMessageResourceName = nameof(AuthDtoValidationMessages.PasswordRequired))]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(AuthDtoValidationMessages),
            ErrorMessageResourceName = nameof(AuthDtoValidationMessages.PasswordRequired))]
        [RegularExpression(ValidationPatterns.PasswordPattern,
            ErrorMessageResourceType = typeof(AuthDtoValidationMessages),
            ErrorMessageResourceName = nameof(AuthDtoValidationMessages.InvalidPassword))]
        public string NewPassword { get; set; } = string.Empty;
    }
}
