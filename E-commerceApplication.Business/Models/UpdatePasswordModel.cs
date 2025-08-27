namespace E_commerceApplication.Business.Models
{
    public class UpdatePasswordModel
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
