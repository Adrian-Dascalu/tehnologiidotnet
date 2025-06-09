using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace tehnologiinet.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message, string attachmentPath = null)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings").Get<SmtpSettings>();

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings.SenderEmail, smtpSettings.SenderName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            if (attachmentPath != null)
            {
                try
                {
                    var attachment = new Attachment(attachmentPath);
                    mailMessage.Attachments.Add(attachment);
                }
                catch (Exception ex)
                {
                    // Log the error or handle it appropriately
                    Console.WriteLine($"Error adding attachment: {ex.Message}");
                }
            }

            using (var client = new SmtpClient(smtpSettings.Server, smtpSettings.Port))
            {
                client.Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password);
                client.EnableSsl = smtpSettings.EnableSsl;
                await client.SendMailAsync(mailMessage);
            }
        }
    }

    public class SmtpSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
    }
} 