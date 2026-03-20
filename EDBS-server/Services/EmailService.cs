using EDBS_server.Services;
using EDBS_server.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EDBS_server.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi email: {ex.Message}");
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }
    }
}