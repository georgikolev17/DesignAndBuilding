namespace DesignAndBuilding.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Common;
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Services.Messaging;
    using DesignAndBuilding.Services.Messaging.DTOs;
    using Microsoft.Extensions.Configuration;

    public class EmailsService : IEmailsService
    {
        private readonly IUsersService usersService;
        private readonly IEmailQueue emailQueue;
        private readonly string baseUrl;

        public EmailsService(IUsersService usersService, IEmailQueue emailQueue, IConfiguration configuration)
        {
            this.usersService = usersService;
            this.emailQueue=emailQueue;
            this.baseUrl = configuration["App:BaseUrl"] ?? string.Empty;
        }

        public async Task SendNewAssignmentNotificationEmailAsync(Assignment assignment)
        {
            var userEmails = await this.usersService.GetEmailsOfUsersOfTypeAsync(assignment.UserType);

            var notificationMessage = MessageTemplates.NewAssignmentCreated(assignment.Building.Name, assignment.UserType.GetDisplayShortName());

            var emailContent = EmailBuilder.BuildNotificationEmail(notificationMessage, $"{this.baseUrl}/assignments/details/{assignment.Id}");

            var emailSubject = EmailSubjects.NewAssignmentCreated;

            var emailDTOs = new List<SendEmailDTO>();

            foreach (var email in userEmails)
            {
                await this.emailQueue.EnqueueAsync(new SendEmailDTO(email, emailContent, emailSubject));
            }
        }
    }
}