using System.ComponentModel.DataAnnotations;
using E_commerceApplication.Resources;
using E_commerceApplication.Validation;

namespace E_commerceApplication.DTOs
{
    public class AuthRequestDto
    {
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), 
            ErrorMessageResourceName = nameof(ValidationMessages.EmailRequired))]
        [RegularExpression(ValidationPatterns.EmailPattern,
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.InvalidEmail))]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.PasswordRequired))]
        [RegularExpression(ValidationPatterns.PasswordPattern,
            ErrorMessageResourceType = typeof(ValidationMessages), 
            ErrorMessageResourceName = nameof(ValidationMessages.InvalidPassword))]
        public string Password { get; set; } = string.Empty;
    }
}
