namespace DesignAndBuilding.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Common.Repositories;
    using DesignAndBuilding.Data.Models;

    public class NotificationsService : INotificationsService
    {
        private readonly IDeletableEntityRepository<Notification> notificationsRepository;

        public NotificationsService(IDeletableEntityRepository<Notification> notificationsRepository)
        {
            this.notificationsRepository = notificationsRepository;
        }

        public async Task AddNotificationAsync(IEnumerable<string> userIds, string message)
        {
            foreach (var id in userIds)
            {
                await this.notificationsRepository.AddAsync(new Notification()
                {
                    Message = message,
                    IsRead = false,
                    UserId = id,
                    IsNew = true,
                });
            }

            await this.notificationsRepository.SaveChangesAsync();
        }

        public async Task<ICollection<Notification>> NewNotificationsForUser(string userId)
        {
            var notifications = this.notificationsRepository.All()
                .Where(x => x.UserId == userId && x.IsNew).ToList();

            foreach (var notification in notifications)
            {
                await this.SetNotificationAsOld(notification);
            }

            return notifications;
        }

        public async Task<bool> DeleteNotification(int notificationId, string userId)
        {
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

        public IEnumerable<Notification> GetNotificationsForUser(string userId)
        {
            return this.notificationsRepository.All().Where(n => n.UserId == userId).OrderByDescending(n => n.CreatedOn);
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
            this.notificationsRepository.All().FirstOrDefault(x => x.Id == notification.Id).IsNew = false;
            await this.notificationsRepository.SaveChangesAsync();
        }
    }
}
