
using IdentityWeb.OptionsModels;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace IdentityWeb.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailOption _emailOptions;
        public EmailService(IOptions<EmailOption> options)
        {
            _emailOptions = options.Value;
        }
        public async Task SendResetEmail(string resetEmailLink, string ToEmail)
        {

            var smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;

            smtpClient.Host = _emailOptions.Host;
            smtpClient.Port = _emailOptions.Port;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new System.Net.NetworkCredential(_emailOptions.Email, _emailOptions.Password);

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_emailOptions.Email);
            mailMessage.To.Add(ToEmail);
            mailMessage.Subject = "Password Reset";
            mailMessage.Body = $"<h4>Click here to reset your password: </h4> <p><a href=\"{resetEmailLink}\">Reset Password</a></p>";
            mailMessage.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mailMessage);
        
        }
    }
}
