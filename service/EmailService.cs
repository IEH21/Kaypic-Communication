using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Web3_kaypic.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var host = _config["Email:Host"];
            var portString = _config["Email:Port"];
            var username = _config["Email:UserName"];
            var password = _config["Email:Password"];
            var from = _config["Email:FromEmail"];

            // Si email non configuré → on sort sans crash
            if (string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(portString) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(from))
            {
                return;
            }

            if (!int.TryParse(portString, out var port))
                return;

            var smtp = new SmtpClient(host)
            {
                Port = port,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            var msg = new MailMessage(from, to, subject, body)
            {
                IsBodyHtml = true
            };

            await smtp.SendMailAsync(msg);
        }
    }
}