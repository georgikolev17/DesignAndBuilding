namespace DesignAndBuilding.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.ViewModels.Notification;

    public interface INotificationsService
    {
        IEnumerable<NotificationViewModel> GetNotificationsForUser(string userId);

        Task AddNotificationAsync(IEnumerable<string> userIds, string message, int assignmentId);

        Task CreateNewAssignmentNotifications(Assignment assignment);

        Task CreateNewBidNotifications(Assignment assignment, string userId, decimal bidPrice);

        Task<bool> DeleteNotification(int notificationId, string userId);

        Task<bool> MarkNotificationAsRead(int notificationId, string userId);

        bool IsNotificationUsers(string userId, int notificationId);

        bool DoesNotificationExists(int notificationId);

        Task<ICollection<Notification>> NewNotificationsForUser(string userId);

        Task SetNotificationAsOld(Notification notifications);
    }
}
