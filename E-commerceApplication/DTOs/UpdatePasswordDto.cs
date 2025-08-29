using E_commerceApplication.Resources;
using E_commerceApplication.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_commerceApplication.DTOs
{
    public class UpdatePasswordDto
    {
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.PasswordRequired))]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.PasswordRequired))]
        [RegularExpression(ValidationPatterns.PasswordPattern,
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.InvalidPassword))]
        public string NewPassword { get; set; } = string.Empty;
    }
}
