namespace DesignAndBuilding.Web.Hubs
{
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;

    public class NotificationsHub : Hub
    {
        private readonly INotificationsService notificationsService;
        private readonly UserManager<ApplicationUser> userManager;

        public NotificationsHub(INotificationsService notificationsService, UserManager<ApplicationUser> userManager)
        {
            this.notificationsService = notificationsService;
            this.userManager = userManager;
        }

        public async Task CheckForNewNotifications()
        {
            do
            {
                var newNotifications = await this.notificationsService.NewNotificationsForUser(this.Context.UserIdentifier);

                if (newNotifications != null)
                {
                    await this.Clients.Caller.SendAsync("RecieveNewNotificationMessage", newNotifications);
                }
            }
            while (true);
        }
    }
}
