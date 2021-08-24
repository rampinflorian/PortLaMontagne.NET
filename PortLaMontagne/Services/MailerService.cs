using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using PortLaMontagne.Forms;

namespace PortLaMontagne.Services
{
    public class MailerService
    {
        private readonly string _authUsername;
        private readonly string _authPassword;
        private readonly MimeMessage _message;

        public MailerService(IConfiguration configuration, MimeMessage message)
        {
            _message = message;

            _authUsername = configuration.GetSection("Mailer").GetSection("Username").Value;
            _authPassword = configuration.GetSection("Mailer").GetSection("Password").Value;

            _message.From.Add(MailboxAddress.Parse("contact@florianrampin.fr"));
            _message.To.Add(MailboxAddress.Parse("contact@florianrampin.fr"));
        }

        public async Task SendContactAsync(ContactForm contactForm)
        {
            _message.Subject = $"PortLaMontagne.fr - Nouveau contact de : {contactForm.Username}";
            
#if DEBUG
            _message.Subject = "#DEBUG - " + _message.Subject;
#endif

            _message.Body = new TextPart("plain")
            {
                Text = $@"Utilisateur : {contactForm.Username}
Email : {contactForm.Email}

Contenu du message :
{contactForm.Content}"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 465, true);

            await client.AuthenticateAsync(
                _authUsername,
                _authPassword
            );

            await client.SendAsync(_message);
            await client.DisconnectAsync(true);
        }
    }
}