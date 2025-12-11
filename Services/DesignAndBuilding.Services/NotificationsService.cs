namespace DesignAndBuilding.Services
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using DesignAndBuilding.Common;
    using DesignAndBuilding.Data.Common.Repositories;
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.ViewModels.Notification;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class NotificationsService : INotificationsService
    {
        private readonly IDeletableEntityRepository<Notification> notificationsRepository;
        private readonly IUsersService usersService;
        private readonly IAssignmentsService assignmentsService;
        private readonly IConfigurationProvider mapper;

        public NotificationsService(IDeletableEntityRepository<Notification> notificationsRepository, IMapper mapper, IUsersService usersService, IAssignmentsService assignmentsService)
        {
            this.notificationsRepository = notificationsRepository;
            this.usersService = usersService;
            this.assignmentsService = assignmentsService;
            this.mapper = mapper.ConfigurationProvider;
        }

        public async Task AddNotificationAsync(IEnumerable<string> userIds, string message, int assignmentId)
        {
            foreach (var id in userIds)
            {
                await this.notificationsRepository.AddAsync(new Notification()
                {
                    Message = message,
                    IsRead = false,
                    UserId = id,
                    IsNew = true,
                    AssignmentId = assignmentId,
                });
            }

            await this.notificationsRepository.SaveChangesAsync();
        }

        public async Task<ICollection<Notification>> NewNotificationsForUser(string userId)
        {
            var notifications = await this.notificationsRepository.All()
                .Where(x => x.UserId == userId && x.IsNew).ToListAsync();

            foreach (var notification in notifications)
            {
                await this.SetNotificationAsOld(notification);
            }

            return notifications;
        }

        public async Task<bool> DeleteNotification(int notificationId, string userId)
        {
            // Check if there is such notification
            if (!this.IsNotificationUsers(userId, notificationId))
            {
                return false;
            }

            this.notificationsRepository
                .All()
                .FirstOrDefault(x => x.Id == notificationId)
                .IsDeleted = true;

            await this.notificationsRepository.SaveChangesAsync();

            return true;
        }

        public bool DoesNotificationExists(int notificationId)
        {
            return this.notificationsRepository.All().Any(x => x.Id == notificationId);
        }

        public IEnumerable<NotificationViewModel> GetNotificationsForUser(string userId)
        {
            return this.notificationsRepository.All()
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedOn)
                .ProjectTo<NotificationViewModel>(this.mapper);
        }

        public bool IsNotificationUsers(string userId, int notificationId)
        {
            return this.notificationsRepository
                .All()
                .FirstOrDefault(x => x.Id == notificationId)
                .UserId == userId;
        }

        public async Task<bool> MarkNotificationAsRead(int notificationId, string userId)
        {
            // Check if notification is for current user
            if (!this.IsNotificationUsers(userId, notificationId))
            {
                return false;
            }

            this.notificationsRepository
                .All()
                .FirstOrDefault(x => x.Id == notificationId)
                .IsRead = true;

            await this.notificationsRepository.SaveChangesAsync();

            return true;
        }

        public async Task SetNotificationAsOld(Notification notification)
        {
            var not = await this.notificationsRepository.All().FirstOrDefaultAsync(x => x.Id == notification.Id);
            not.IsNew = false;
            await this.notificationsRepository.SaveChangesAsync();
        }

        public async Task CreateNewAssignmentNotifications(Assignment assignment)
        {
            var notificationMessage = MessageTemplates.NewAssignmentCreated(assignment.Building.Name, assignment.UserType.GetDisplayShortName());

            var userIds = (await this.usersService.GetIdsOfUsersOfTypeAsync(assignment.UserType)).ToList();

            await this.AddNotificationAsync(userIds, notificationMessage, assignment.Id);
        }

        public async Task CreateNewBidNotifications(Assignment assignment, string userId, decimal bidPrice)
        {
            var usersToSendNotification = this.assignmentsService.GetAllUsersBidInAssignment(assignment.Id);

            // Assignment creator should also recieve notification
            usersToSendNotification.Add(assignment.Building.ArchitectId);

            // User who placed bid will recieve another notification
            usersToSendNotification.Remove(userId);
            var userPlacedBid = new List<string>() { userId };
            await this.AddNotificationAsync(userPlacedBid, MessageTemplates.YouSubmittedNewBid(assignment.Building.Name, assignment.UserType.GetDisplayShortName(), bidPrice), assignment.Id);

            // All other users placed bids for this assignment will recieve this notification
            await this.AddNotificationAsync(usersToSendNotification, MessageTemplates.NewBidSubmitted(assignment.Building.Name, assignment.UserType.GetDisplayShortName(), bidPrice), assignment.Id);
        }
    }
}
