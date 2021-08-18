namespace DesignAndBuilding.Web.Hubs
{
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;

    public class NotificationsHub : Hub
    {
        private readonly INotificationsService notificationsService;

        public NotificationsHub(INotificationsService notificationsService)
        {
            this.notificationsService = notificationsService;
        }

        public async Task CheckForNewNotifications()
        {
            do
            {
                var newNotifications = await this.notificationsService.NewNotificationsForUser(this.Context.UserIdentifier);

                var newNotificationsMessages = newNotifications.Select(x => x.Message);

                if (newNotificationsMessages.Any())
                {
                    await this.Clients.Caller.SendAsync("RecieveNewNotificationMessage", newNotificationsMessages);
                }
            }
            while (true);
        }
    }
}
