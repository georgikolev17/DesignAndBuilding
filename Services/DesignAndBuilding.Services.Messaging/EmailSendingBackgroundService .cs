using DesignAndBuilding.Services.Messaging.DTOs;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;

namespace DesignAndBuilding.Services.Messaging
{
    public class EmailSendingBackgroundService : BackgroundService
    {
        private readonly IEmailQueue _queue;
        private readonly IEmailSender _emailSender;
        private readonly SmtpOptions _options;

        public EmailSendingBackgroundService(IEmailQueue queue, IOptions<SmtpOptions> options, IEmailSender emailSender)
        {
            this._queue = queue;
            this._options = options.Value;
            this._emailSender = emailSender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await foreach (SendEmailDTO email in _queue.DequeueAllAsync(stoppingToken))
                {
                    if (stoppingToken.IsCancellationRequested)
                        break;

                    try
                    {
                        await _emailSender.SendEmailAsync(email);
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Application shutting down normally
            }
        }
    }
}
