namespace E_commerceApplication.Business.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailConfirmationAsync(string toEmail, string link);
    }
}
