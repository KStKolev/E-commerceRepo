using E_commerceApplication.Resources;
using E_commerceApplication.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_commerceApplication.DTOs
{
    public class UserProfileDto
    {
        [Required(ErrorMessageResourceType = typeof(AuthDtoValidationMessages), 
            ErrorMessageResourceName = nameof(AuthDtoValidationMessages.UserNameRequired))]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(AuthDtoValidationMessages),
            ErrorMessageResourceName = nameof(AuthDtoValidationMessages.PhoneNumberRequired))]
        [RegularExpression(ValidationPatterns.PhoneNumberPattern,
            ErrorMessageResourceType = typeof(AuthDtoValidationMessages), 
            ErrorMessageResourceName = nameof(AuthDtoValidationMessages.InvalidPhoneNumber))]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(AuthDtoValidationMessages),
            ErrorMessageResourceName = nameof(AuthDtoValidationMessages.AddressDeliveryRequired))]
        public string AddressDelivery { get; set; } = string.Empty;
    }
}
