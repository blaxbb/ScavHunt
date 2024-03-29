﻿using Microsoft.AspNetCore.Identity.UI.Services;
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
                From = new EmailAddress(options.FromEmail, "SIGGRAPH 2023 - Scavenger Hunt"),
                Subject = subject,
                PlainTextContent = message.Replace("<br />", "\n"),
                HtmlContent = message,
            };

            msg.AddTo(new EmailAddress(email));

            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);

            logger.LogInformation(response.IsSuccessStatusCode ? $"Email to {email} queued" : $"Email to {email} failed");
        }

        public async Task SendEmailAsync(List<string> emails, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(options.Key))
            {
                throw new Exception("Send grid key is not defined.");
            }

            var client = new SendGridClient(options.Key);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(options.FromEmail, "SIGGRAPH 2023 - Scavenger Hunt"),
                Subject = subject,
                PlainTextContent = message.Replace("<br />", "\n"),
                HtmlContent = message,
            };

            msg.Personalizations = new List<Personalization>();

            msg.Personalizations.AddRange(emails.Select(e =>
            
                new Personalization()
                {
                    Tos = new() { new EmailAddress(e) }
                }
            ));

            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);

            logger.LogInformation(response.IsSuccessStatusCode ? $"Bulk email to {emails.Count} queued" : $"Bulk emails to {emails.Count} failed");
        }
    }
}
