using DesignAndBuilding.Services.Messaging.DTOs;
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

        public async Task SendEmailAsync(SendEmailDTO email)
        {
            var message = this.prepareMessage(email);

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

        public async Task SendMultipleEmailsAsync(IEnumerable<SendEmailDTO> emails)
        {
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

            //CONNECT
            await client.ConnectAsync(_options.Host, _options.Port, secureSocketOptions);

            // AUTHENTICATE
            if (!string.IsNullOrEmpty(_options.User))
            {
                await client.AuthenticateAsync(_options.User, _options.Password);
            }

            foreach (var email in emails)
            {
                // SEND
                var message = this.prepareMessage(email);
                await client.SendAsync(message);
            }

            // DISCONNECT
            await client.DisconnectAsync(true);
            
        }

        private MimeMessage prepareMessage(SendEmailDTO email)
        {
            var message = new MimeMessage();

            // FROM
            message.From.Add(new MailboxAddress(this._options.FromName, this._options.FromEmail));

            // TO
            message.To.Add(new MailboxAddress(email.To, email.To));

            // SUBJECT
            message.Subject = email.Subject;

            // BODY BUILDER
            var builder = new BodyBuilder
            {
                HtmlBody = email.HtmlContent
            };

            // ATTACHMENTS
            if (email.Attachments != null)
            {
                foreach (var attachment in email.Attachments)
                {
                    builder.Attachments.Add(attachment.FileName, attachment.Content, ContentType.Parse(attachment.MimeType));
                }
            }

            message.Body = builder.ToMessageBody();

            return message;
        }
    }
}
