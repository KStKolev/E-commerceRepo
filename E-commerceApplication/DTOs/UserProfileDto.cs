using E_commerceApplication.Resources;
using E_commerceApplication.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_commerceApplication.DTOs
{
    public class UserProfileDto
    {
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), 
            ErrorMessageResourceName = nameof(ValidationMessages.UserNameRequired))]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.PhoneNumberRequired))]
        [RegularExpression(ValidationPatterns.PhoneNumberPattern,
            ErrorMessageResourceType = typeof(ValidationMessages), 
            ErrorMessageResourceName = nameof(ValidationMessages.InvalidPhoneNumber))]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.AddressDeliveryRequired))]
        public string AddressDelivery { get; set; } = string.Empty;
    }
}
