namespace DesignAndBuilding.Tests.Controllers
{
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.Controllers.Api;
    using MyTested.AspNetCore.Mvc;
    using System.Linq;
    using Xunit;

    public class NotificationsApiControllerTest
    {
        [Fact]
        public void ControllerShouldHaveApiRouteAndAuthorizeAttributes()
            => MyController<NotificationsApiController>
                .ShouldHave()
                .Attributes(a =>
                {
                    a.SpecifyingRoute("api/notifications");
                    a.IndicatingApiController();
                    a.RestrictingForAuthorizedRequests();
                });

        //Delete

        [Fact]
        public void DeleteShouldHaveRouteAttribute()
            => MyController<NotificationsApiController>
                .Calling(c => c.Delete("1", "1"))
                .ShouldHave()
                .ActionAttributes(a => a.SpecifyingRoute("delete"));

        [Fact]
        public void DeleteShouldReturnNotFoundWhenNotificationIdIsInvalid()
            => MyController<NotificationsApiController>
                .Calling(c => c.Delete("1", "1"))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void DeleteShouldRetturnCorrectJsonIfUserCantDeleteNotification()
            => MyController<NotificationsApiController>
                .Instance()
                .WithData(GetUser(UserType.ElectroEngineer))
                .WithData(GetNotificationForUser("2"))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Delete("1", "1"))
                .ShouldReturn()
                .Json(false);

        [Fact]
        public void DeleteShouldRetturnCorrectJson()
            => MyController<NotificationsApiController>
                .Instance()
                .WithData(GetUser(UserType.ElectroEngineer, userId: "2"))
                .WithData(GetNotificationForUser("2"))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Delete("1", "2"))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Notification>(set =>
                    {
                        Assert.Equal(0, set.Count());
                    }))
                .AndAlso()
                .ShouldReturn()
                .Json(true);

        //MarkAsRead

        [Fact]
        public void MarkAsReadShouldHaveRouteAttribute()
            => MyController<NotificationsApiController>
                .Calling(c => c.MarkAsRead("1", "1"))
                .ShouldHave()
                .ActionAttributes(a => a.SpecifyingRoute("markasread"));

        [Fact]
        public void MarkAsReadShouldReturnNotFoundWhenNotificationIdIsInvalid()
            => MyController<NotificationsApiController>
                .Calling(c => c.MarkAsRead("1", "1"))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void MarkAsReadShouldRetturnCorrectJsonIfUserCantDeleteNotification()
            => MyController<NotificationsApiController>
                .Instance()
                .WithData(GetUser(UserType.ElectroEngineer))
                .WithData(GetNotificationForUser("2"))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.MarkAsRead("1", "1"))
                .ShouldReturn()
                .Json(false);

        [Fact]
        public void MarkAsReadShouldRetturnCorrectJson()
            => MyController<NotificationsApiController>
                .Instance()
                .WithData(GetUser(UserType.ElectroEngineer, userId: "2"))
                .WithData(GetNotificationForUser("2"))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.MarkAsRead("1", "2"))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Notification>(set =>
                    {
                        Assert.True(set.SingleOrDefault(x => x.Id == 1).IsRead);
                    }))
                .AndAlso()
                .ShouldReturn()
                .Json(true);

        // Static methods
        private static ApplicationUser GetUser(UserType designerType, string userId = ControllerConstants.UserId, string username = ControllerConstants.Username)
        {
            var user = new ApplicationUser() { UserType = designerType, Id = userId, UserName = username, };

            return user;
        }

        private static Notification GetNotificationForUser(string userId = ControllerConstants.UserId)
        {
            return new Notification()
            {
                UserId = userId,
            };
        }
    }
}
