namespace DesignAndBuilding.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using DesignAndBuilding.Data.Common.Repositories;
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.ViewModels.Notification;
    using Microsoft.EntityFrameworkCore;

    public class NotificationsService : INotificationsService
    {
        private readonly IDeletableEntityRepository<Notification> notificationsRepository;
        private readonly IConfigurationProvider mapper;

        public NotificationsService(IDeletableEntityRepository<Notification> notificationsRepository, IMapper mapper)
        {
            this.notificationsRepository = notificationsRepository;
            this.mapper = mapper.ConfigurationProvider;
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
    }
}
