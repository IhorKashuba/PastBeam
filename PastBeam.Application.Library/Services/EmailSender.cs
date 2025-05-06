using System.Net.Mail;
using System.Net;

namespace PastBeam.Application.Library.Services // або PastBeam.Core.Library.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _smtpHost = "smtp.gmail.com"; // Заміни на ваш SMTP сервер
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser = "your-email@example.com"; // Твоя електронна пошта
        private readonly string _smtpPassword = "your-email-password"; // Твій пароль для пошти

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUser),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            using (var smtpClient = new SmtpClient(_smtpHost, _smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}

