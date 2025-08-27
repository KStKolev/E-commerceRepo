using System.ComponentModel.DataAnnotations;

namespace E_commerceApplication
{
    public class SmtpSettings
    {
        [Required]
        public string Host { get; set; } = string.Empty;

        public int Port { get; set; }

        [Required]
        public string User { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
