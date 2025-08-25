using System.ComponentModel.DataAnnotations;

namespace E_commerceApplication
{
    public class SmtpSettings
    {
        [Required]
        public string Host { get; set; } = string.Empty;

        [Range(587, 587, ErrorMessage = "Port must be 587.")]
        public int Port { get; set; }

        [Required, EmailAddress]
        public string User { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
