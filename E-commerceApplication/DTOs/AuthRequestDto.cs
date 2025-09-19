using System.ComponentModel.DataAnnotations;
using E_commerceApplication.Resources;
using E_commerceApplication.Validation;

namespace E_commerceApplication.DTOs
{
    public class AuthRequestDto
    {
        [Required(ErrorMessageResourceType = typeof(AuthDtoValidationMessages), 
            ErrorMessageResourceName = nameof(AuthDtoValidationMessages.EmailRequired))]
        [RegularExpression(ValidationPatterns.EmailPattern,
            ErrorMessageResourceType = typeof(AuthDtoValidationMessages),
            ErrorMessageResourceName = nameof(AuthDtoValidationMessages.InvalidEmail))]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(AuthDtoValidationMessages),
            ErrorMessageResourceName = nameof(AuthDtoValidationMessages.PasswordRequired))]
        [RegularExpression(ValidationPatterns.PasswordPattern,
            ErrorMessageResourceType = typeof(AuthDtoValidationMessages), 
            ErrorMessageResourceName = nameof(AuthDtoValidationMessages.InvalidPassword))]
        public string Password { get; set; } = string.Empty;
    }
}
