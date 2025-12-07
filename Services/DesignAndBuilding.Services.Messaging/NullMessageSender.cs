namespace DesignAndBuilding.Services.Messaging
{
    using DesignAndBuilding.Services.Messaging.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class NullMessageSender : IEmailSender
    {
        public Task SendEmailAsync(
            string to,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment> attachments = null)
        {
            return Task.CompletedTask;
        }

        public Task SendEmailAsync(SendEmailDTO email)
        {
            throw new NotImplementedException();
        }

        public Task SendMultipleEmailsAsync(IEnumerable<SendEmailDTO> emails)
        {
            throw new NotImplementedException();
        }
    }
}
