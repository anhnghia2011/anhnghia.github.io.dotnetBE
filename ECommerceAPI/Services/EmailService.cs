using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using NikeShoeStoreApi.Models;

namespace ECommerceAPI.Services
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _senderEmail = "nghiangongcuongvai@gmail.com";
        private readonly string _appPassword = "iuwxxmsbxtbjjcbl";

        public async Task SendFeedbackEmail(Feedback feedback)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(feedback.Name, _senderEmail));
            message.To.Add(new MailboxAddress("Admin", "nguyenanhnghia81@gmail.com"));
            message.Subject = "New Feedback Received";

            message.Body = new TextPart("plain")
            {
                Text = $"Name: {feedback.Name}\nEmail: {feedback.Email}\nMessage: {feedback.Message}"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(_senderEmail, _appPassword);

                await client.SendAsync(message);

                await client.DisconnectAsync(true);
            }
        }
    }
}
