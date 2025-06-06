﻿namespace DesignAndBuilding.Web.Controllers.Api
{
    using System.Threading.Tasks;

    using DesignAndBuilding.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/notifications")]
    [Authorize]
    public class NotificationsApiController : BaseController
    {
        private readonly INotificationsService notificationsService;

        public NotificationsApiController(INotificationsService notificationsService)
        {
            this.notificationsService = notificationsService;
        }

        [Route("delete")]
        public async Task<IActionResult> Delete(string notificationId, string userId)
        {
            int notificationIdNumber = int.Parse(notificationId);

            if (!this.notificationsService.DoesNotificationExists(notificationIdNumber))
            {
                return this.NotFound();
            }

            if (await this.notificationsService.DeleteNotification(notificationIdNumber, userId))
            {
                return this.Json(true);
            }

            return this.Json(false);
        }

        [Route("markasread")]
        public async Task<IActionResult> MarkAsRead(string notificationId, string userId)
        {
            int notificationIdNumber = int.Parse(notificationId);

            if (!this.notificationsService.DoesNotificationExists(notificationIdNumber))
            {
                return this.NotFound();
            }

            if (await this.notificationsService.MarkNotificationAsRead(notificationIdNumber, userId))
            {
                return this.Json(true);
            }

            return this.Json(false);
        }
    }
}
