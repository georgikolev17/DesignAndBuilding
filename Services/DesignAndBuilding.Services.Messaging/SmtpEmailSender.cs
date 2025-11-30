using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesignAndBuilding.Services.Messaging
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpOptions _options;

        public SmtpEmailSender(IOptions<SmtpOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendEmailAsync(
            string to,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment> attachments = null!)
        {
            var message = new MimeMessage();

            // FROM
            message.From.Add(new MailboxAddress(this._options.FromName, this._options.FromEmail));

            // TO
            message.To.Add(new MailboxAddress(to, to));

            // SUBJECT
            message.Subject = subject;

            // BODY BUILDER
            var builder = new BodyBuilder
            {
                HtmlBody = htmlContent
            };

            // ATTACHMENTS
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    builder.Attachments.Add(attachment.FileName, attachment.Content, ContentType.Parse(attachment.MimeType));
                }
            }

            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();

            SecureSocketOptions secureSocketOptions;

            if (_options.Port == 465)
            {
                // SSL from the start (implicit TLS)
                secureSocketOptions = SecureSocketOptions.SslOnConnect;
            }
            else if (_options.Port == 587)
            {
                // STARTTLS on submission port
                secureSocketOptions = SecureSocketOptions.StartTls;
            }
            else
            {
                // Fallback: negotiate the best the server supports
                secureSocketOptions = SecureSocketOptions.Auto;
            }

            await client.ConnectAsync(_options.Host, _options.Port, secureSocketOptions);

            // AUTHENTICATE
            if (!string.IsNullOrEmpty(_options.User))
            {
                await client.AuthenticateAsync(_options.User, _options.Password);
            }

            // SEND
            await client.SendAsync(message);

            // DISCONNECT
            await client.DisconnectAsync(true);
        }
    }
}
