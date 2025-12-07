namespace DesignAndBuilding.Services.Messaging
{
    using DesignAndBuilding.Services.Messaging.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEmailSender
    {
        Task SendEmailAsync(SendEmailDTO email);

        Task SendMultipleEmailsAsync(IEnumerable<SendEmailDTO> emails);
    }
}
