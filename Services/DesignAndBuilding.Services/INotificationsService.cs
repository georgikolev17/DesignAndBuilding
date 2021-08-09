namespace DesignAndBuilding.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Models;

    public interface INotificationsService
    {
        IEnumerable<Notification> GetNotificationsForUser(string userId);

        Task AddNotificationAsync(IEnumerable<string> userIds, string message);

        Task<bool> DeleteNotification(int notificationId, string userId);

        Task<bool> MarkNotificationAsRead(int notificationId, string userId);

        bool IsNotificationUsers(string userId, int notificationId);
    }
}
