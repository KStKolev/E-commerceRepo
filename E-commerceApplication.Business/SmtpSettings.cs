using System.ComponentModel.DataAnnotations;

namespace E_commerceApplication
{
    public class SmtpSettings
    {
        [Required]
        public string Host { get; set; } = string.Empty;

        // Default SMTP port is 578
        public int Port { get; set; } = 578;

        [Required]
        public string User { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
