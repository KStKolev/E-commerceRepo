using System.ComponentModel.DataAnnotations;

namespace E_commerceApplication.DTOs
{
    public class AuthCredentialsRequestDto
    {
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            ErrorMessage = "Invalid email format"
        )]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 characters, include upper/lowercase, number, and special character"
        )]
        public string Password { get; set; } = string.Empty;
    }
}
