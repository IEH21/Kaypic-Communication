using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace web3_kaypic.service
{
    public class MailKitOptions
    {
        public string Host { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string FromName { get; set; } = "Kaypic Support";
        public string FromEmail { get; set; } = "";
    }
    public class MailKitEmailSender : IEmailSender
    {
        private readonly MailKitOptions _options;
        public MailKitEmailSender(IOptions<MailKitOptions> options)
        {
            _options = options.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_options.FromName, _options.FromEmail));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = subject;
            // Le contenu HTML du message
            message.Body = new TextPart("html")
            {
                Text = htmlMessage
            };
            using var client = new SmtpClient();
            try
            {
                // Connexion au serveur Gmail
                await client.ConnectAsync(_options.Host, _options.Port,
               SecureSocketOptions.StartTls);
                // Authentification avec Gmail
                await client.AuthenticateAsync(_options.UserName, _options.Password);
                // Envoi du message
                await client.SendAsync(message);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }

        }

    }
}