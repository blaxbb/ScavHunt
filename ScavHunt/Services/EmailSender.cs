using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ScavHunt.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly SendGridOptions options;
        private readonly ILogger<EmailSender> logger;

        public EmailSender(IOptions<SendGridOptions> options, ILogger<EmailSender> logger)
        {
            this.options = options.Value;
            this.logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(options.Key))
            {
                throw new Exception("Send grid key is not defined.");
            }

            var client = new SendGridClient(options.Key);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(options.FromEmail),
                Subject = subject,
                PlainTextContent = message.Replace("<br />", "\n"),
                HtmlContent = message,
            };

            msg.AddTo(new EmailAddress(email));

            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);

            logger.LogInformation(response.IsSuccessStatusCode ? $"Email to {email} queued" : $"Email to {email} failed");
        }
    }
}
