using System;
using System.Collections.Generic;
using System.Text;

namespace DesignAndBuilding.Web.ViewModels.Notification
{
    public class AllNotificationsViewModel
    {
        public string UserId { get; set; }

        public IEnumerable<NotificationViewModel> Notifications { get; set; }
    }
}
