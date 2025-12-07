using brevo_csharp.Api;
using brevo_csharp.Client;
using brevo_csharp.Model;
using DesignAndBuilding.Services.Messaging.DTOs;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndBuilding.Services.Messaging
{
    public class BrevoEmailSender : IEmailSender
    {
        private readonly BrevoOptions _options;
        private readonly TransactionalEmailsApi _api;

        public BrevoEmailSender(IOptions<BrevoOptions> options)
        {
            _options = options.Value;

            // Configure API key for Brevo
            brevo_csharp.Client.Configuration.Default.ApiKey.Clear();
            brevo_csharp.Client.Configuration.Default.ApiKey.Add("api-key", _options.ApiKey);

            // Create the Brevo transactional email API client
            _api = new TransactionalEmailsApi();
        }

        public async System.Threading.Tasks.Task SendEmailAsync(SendEmailDTO email)
        {
            var toList = new List<SendSmtpEmailTo>
            {
                new SendSmtpEmailTo(email.To, email.To)
            };

            var message = new SendSmtpEmail(
                sender: new SendSmtpEmailSender(_options.FromName, _options.FromEmail),
                to: toList,
                subject: email.Subject,
                htmlContent: email.HtmlContent
            );

            // Handle attachments if you use them
            if (email.Attachments != null)
            {
                message.Attachment = new List<SendSmtpEmailAttachment>();

                foreach (var att in email.Attachments)
                {
                    message.Attachment.Add(new SendSmtpEmailAttachment
                    {
                        Name = att.FileName,
                        Content = att.Content
                    });
                }
            }

            await _api.SendTransacEmailAsync(message);
        }

        public async System.Threading.Tasks.Task SendMultipleEmailsAsync(IEnumerable<SendEmailDTO> emails)
        {
            foreach (var email in emails)
            {
                await SendEmailAsync(email);
            }
        }
    }
}
