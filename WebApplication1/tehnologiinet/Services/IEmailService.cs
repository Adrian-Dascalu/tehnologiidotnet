namespace tehnologiinet.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message, string attachmentPath = null);
    }
} 