﻿namespace DesignAndBuilding.Tests.Controllers
{
    using Xunit;
    using global::DesignAndBuilding.Web.Controllers;
    using MyTested.AspNetCore.Mvc;
    using global::DesignAndBuilding.Data.Models;
    using System.Collections.Generic;
    using System.Linq;
    using global::DesignAndBuilding.Web.ViewModels.Assignment;
    using Moq;
    using Microsoft.AspNetCore.Identity;
    using DesignAndBuilding.Tests.Mocks;
    using Microsoft.Extensions.Options;
    using System;
    using Microsoft.Extensions.Logging;
    using global::DesignAndBuilding.Web.ViewModels;
    using global::DesignAndBuilding.Web.ViewModels.Notification;

    public class HomeControllerTest
    {
        [Fact]
        public void HomeControllerShouldHaveNoAttributes()
            => MyController<HomeController>
                .ShouldHave()
                .NoAttributes();

        [Fact]
        public void PrivacyShouldReturnViewAndShouldHaveAuthorizeAttribute()
            => MyController<HomeController>
                .Instance()
                .Calling(c => c.Privacy())
                .ShouldHave()
                .ActionAttributes(a => a.RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData(DesignerType.ElectroEngineer, 10)]
        // Default user is Electro engineer so he shouldn't see other engineer's assignments
        [InlineData(DesignerType.BuildingConstructionEngineer, 0)]
        [InlineData(DesignerType.PlumbingEngineer, 0)]
        [InlineData(DesignerType.HVACEngineer, 0)]
        [InlineData(DesignerType.Other, 0)]
        public void IndexShouldReturnCorrectViewWithCoreectDataAndModelForLoggedUserAndShouldHaveAttributes(DesignerType designerType, int expectedAssignmentsCount)
        {
            MyController<HomeController>
               .Instance()
                .WithData(Get10Assignments(designerType))
                .WithData(GetUser(DesignerType.Architect))
               .Calling(c => c.Index())
               .ShouldHave()
               .ActionAttributes()
               .AndAlso()
               .ShouldReturn()
               .View(view => view
                   .WithModelOfType<EngineerAssignmentsViewModel>()
                   .Passing(m => Assert.Equal(expectedAssignmentsCount, m.Assignments.Count)));
        }

        [Fact]
        public void ErrorShouldReturnViewWithCorrectViewModelAndShouldHaveCachingResponseAttribute()
            => MyController<HomeController>
                .Instance()
                .Calling(c => c.Error())
                .ShouldHave()
                .ActionAttributes(a => a.CachingResponse())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());

        [Theory]
        [InlineData(ControllerConstants.UserId, 10)]
        [InlineData("2", 0)]
        public void MyBidsShouldHaveAttributesAttributeAndShouldReturnCorrectViewWithCorrectModel(string userIdForbids, int bidsCount)
            => MyController<HomeController>
                .Instance()
                .WithData(Get10AssignmentsWhereUserBid(userIdForbids))
                .WithData(GetUser(DesignerType.Architect))
                .Calling(c => c.MyBids())
                .ShouldHave()
                .ActionAttributes(c => c.RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<EngineerAssignmentsViewModel>()
                .Passing(v => Assert.Equal(v.Assignments.Count, bidsCount)));

        [Theory]
        //[InlineData(ControllerConstants.UserId, 10)]
        [InlineData("2", 0)]
        public void NotificationsShouldHaveAttributesAttributeAndShouldReturnCorrectViewWithCorrectModel(string userIdForNotifications, int notificationsCount)
            => MyController<HomeController>
                .Instance()
                .WithData(Get10NotificationsForUser(userIdForNotifications))
                .WithData(GetUser(DesignerType.Architect))
                .Calling(c => c.Notifications())
                .ShouldHave()
                .ActionAttributes(c => c.RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<AllNotificationsViewModel>()
                .Passing(v => Assert.Equal(v.Notifications.Count(), notificationsCount)));

        private static IEnumerable<Assignment> Get10Assignments(DesignerType designerType)
        {
            var assignments = new List<Assignment>();
            for (int i = 1; i <= 10; i++)
            {
                assignments.Add(new Assignment()
                {
                    Building = new Building
                    {
                        Name = ControllerConstants.BuildingName,
                        ArchitectId = ControllerConstants.UserId,
                    },
                    CreatedOn = DateTime.UtcNow,
                    Description = ControllerConstants.Description,
                    DesignerType = designerType,
                    EndDate = DateTime.UtcNow + TimeSpan.FromDays(5),
                });
            }

            return assignments;
        }

        private static ApplicationUser GetUser(DesignerType designerType, string email = "admin@dab.com")
        {
            return new ApplicationUser()
            {
                Id = ControllerConstants.UserId,
                FirstName = ControllerConstants.UserFirstName,
                LastName = ControllerConstants.UserLastName,
                DesignerType = designerType,
                Email = email,
            };
        }

        private static List<Assignment> Get10AssignmentsWhereUserBid(string userId)
        {
            List<Assignment> assignments = Enumerable.Range(0, 10).Select(b => new Assignment() { Building = new Building() { ArchitectId = ControllerConstants.UserId } }).ToList();

            foreach (var assignment in assignments)
            {
                assignment.Bids.Add(new Bid() { DesignerId = userId });
            }

            return assignments.ToList();
        }

        private static IEnumerable<Notification> Get10NotificationsForUser(string userId)
        {
            return Enumerable.Range(0, 10).Select(x => new Notification() { UserId = userId });
        }
    }
}
