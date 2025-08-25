using E_commerceApplication.Business.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace E_commerceApplication.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailConfirmationAsync(string toEmail, string link)
        {
            using SmtpClient client = new(_smtpSettings.Host, _smtpSettings.Port)
            {
                Credentials = new NetworkCredential(_smtpSettings.User, _smtpSettings.Password),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_smtpSettings.User, "E-commerceApplication"),
                Subject = "Confirm your email",
                Body = $"Please confirm your account by clicking this link: <a href='{link}'>Confirm Email</a>",
                IsBodyHtml = true
            };

            mail.To.Add(toEmail);
            await client.SendMailAsync(mail);
        }
    }
}
