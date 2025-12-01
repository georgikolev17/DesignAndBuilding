namespace DesignAndBuilding.Tests.Controllers
{
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.Controllers;
    using DesignAndBuilding.Web.ViewModels;
    using DesignAndBuilding.Web.ViewModels.Assignment;
    using DesignAndBuilding.Web.ViewModels.Bid;
    using Microsoft.AspNetCore.Http;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class AssignmentsControllerTest
    {
        [Fact]
        public void AssignmentsControllerShouldHaveAuthorizeAttribute()
            => MyController<AssignmentsController>
                .ShouldHave()
                .Attributes(a => a.RestrictingForAuthorizedRequests());

        // Create - GET

        [Fact]
        public void CreateGetShouldReturnView()
            => MyController<AssignmentsController>
                .Instance()
                .Calling(c => c.Create())
                .ShouldReturn()
                .View();

        // Create - POST

        [Fact]
        public void CreatePostShouldReturnViewWithSameModelWhenModelStateInvalid()
            => MyController<AssignmentsController>
                .Calling(c => c.Create(With.Default<AssignmentInputModel>()))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(With.Default<AssignmentInputModel>());

        [Fact]
        public void CreatePostShouldHaveHttpPostAttributeAndShouldCreateAssignmentAndShouldRedirectToCorrectBuildingWhenModelStateValid()
            => MyController<AssignmentsController>
                .Calling(c => c.Create(new AssignmentInputModel()
                {
                    BuildingId = 1,
                    EndDate = DateTime.UtcNow + TimeSpan.FromDays(5),
                    Description = new List<IFormFile>(),
                    UserType = UserType.ElectroEngineer,
                }))
                .ShouldHave()
                .ActionAttributes(a => a.RestrictingForHttpMethod(HttpMethod.Post))
                .AndAlso()
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldHave()
                .Data(data => data
                    .WithSet<Assignment>(set =>
                    {
                        Assert.NotNull(set);
                        Assert.NotNull(set.SingleOrDefault(a => a.BuildingId == 1));
                    }))
                .AndAlso()
                .ShouldReturn()
                .Redirect(r => r
                    .To<BuildingsController>(c => c.Details(1)));

        // Details - GET

        [Fact]
        public void DetailsGetShouldReturnNotFoundWhenAssignmentIdIsInvalid()
            => MyController<AssignmentsController>
                .Calling(c => c.Details(1))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void DetailsShouldReturnViewWithCorrectDataAndModelWhenIdIsValid()
            => MyController<AssignmentsController>
                .Instance()
                .WithData(GetAssignment(1))
                .WithData(GetUser(designerType: UserType.ElectroEngineer))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Details(1))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<AssignmentDetailsViewModel>());

        // Details - POST

        [Fact]
        public void DetailsPostShouldHaveHttpPostAttribute()
            => MyController<AssignmentsController>
                .Calling(c => c.Details(With.Empty<PlaceBidViewModel>()))
                .ShouldHave()
                .ActionAttributes(a => a.RestrictingForHttpMethod(HttpMethod.Post));

        [Fact]
        public void DetailsPostShouldReturnNotFoundForInvalidAssignment()
            => MyController<AssignmentsController>
                .Calling(c => c.Details(With.Empty<PlaceBidViewModel>()))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void DetailsPostShouldReturnErrorViewWithCorrectErrorMessageWhenModelStateIsInvalid()
            => MyController<AssignmentsController>
                .Instance()
                .WithData(GetAssignment(1, UserType.ElectroEngineer))
                .WithData(GetUser(designerType: UserType.ElectroEngineer))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Details(new PlaceBidViewModel() { Id = 1, BidPrice = 21 }))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View("Error", new ErrorViewModel() { ErrorMessage = "Невалидно наддаване" });

        [Fact]
        public void DetailsPostShouldReturnErrorViewWithCorrectErrorMessageWhenAssignmentIsNotForCurrentuserDesignerType()
            => MyController<AssignmentsController>
                .Instance()
                .WithData(GetAssignment(1))
                .WithData(GetUser(designerType: UserType.ElectroEngineer))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Details(new PlaceBidViewModel() { Id = 1, BidPrice = 21 }))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View("Error", new ErrorViewModel() { ErrorMessage = "Само други могат да наддават за това задание!" });

        [Fact]
        public void DetailsPostShouldCreateBidAndSendNotificationsToAllUsersWhoHaveBidForThisAssignmentAndToAssignmentsCreator()
            => MyController<AssignmentsController>
                .Instance()
                .WithData(GetAssignment(designerType: UserType.ElectroEngineer))
                .WithData(Get10UsersBidForAssignment())
                .WithData(GetUser(designerType: UserType.ElectroEngineer))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Details(new PlaceBidViewModel() { BidPrice = 1, Id = 1 }))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldHave()
                .Data(data => data
                    .WithSet<Bid>(set =>
                    {
                        Assert.NotNull(set.SingleOrDefault(b => b.AssignmentId == 1 && b.Price == 1));
                    })
                    .WithSet<Notification>(set =>
                    {
                        Assert.NotNull(set.SingleOrDefault(n => n.UserId == ControllerConstants.UserId));
                        Assert.Equal(12, set.Count());
                    }))
                .AndAlso()
                .ShouldReturn()
                .Redirect(r => r.To<AssignmentsController>(c => c.Details(1)));

        //Edit - GET

        [Fact]
        public void EditGetShouldReturnNotFoundIfIdInvalid()
            => MyController<AssignmentsController>
                .Calling(c => c.Edit(1))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void EditGetSHouldReturnErrorViewWithCorrectExceptionMessageIfUserHasntCreatedAssignment()
            => MyController<AssignmentsController>
                .Instance()
                .WithData(GetUser(designerType: UserType.ElectroEngineer))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .WithData(GetAssignment())
                .Calling(c => c.Edit(1))
                .ShouldReturn()
                .View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал заданието, може да го редактира" });

        [Fact]
        public void EditGetShouldReturnViewWithCorrectModelAndDataIfUserHasCreatedAssignment()
            => MyController<AssignmentsController>
                .Instance()
                .WithData(GetUser(designerType: UserType.ElectroEngineer))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .WithData(GetAssignment(architectId: ControllerConstants.UserId))
                .Calling(c => c.Edit(1))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<AssignmentInputModel>(model => model.UserType == UserType.Other));

        //Edit - POST

        [Fact]
        public void EditPostShouldHaveHttpPostAttribute()
            => MyController<AssignmentsController>
                .Calling(c => c.Edit(With.Empty<int>(), With.Empty<AssignmentInputModel>()))
                .ShouldHave()
                .ActionAttributes(a => a.RestrictingForHttpMethod(HttpMethod.Post));

        [Fact]
        public void EditPostShouldReturnErrorViewWithCorrectExceptionMessageIfUserHasntCreatedAssignment()
            => MyController<AssignmentsController>
                .Instance()
                .WithData(GetAssignment())
                .WithData(GetUser(designerType: UserType.ElectroEngineer))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Edit(1, new AssignmentInputModel() { Description = new List<IFormFile>(), EndDate = DateTime.Now + TimeSpan.FromDays(5), BuildingId = 1, UserType = UserType.Other }))
                .ShouldReturn()
                .View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал заданието, може да го редактира" });

        [Fact]
        public void InvalidAssignmentIdShouldReturnNotFoundError()
            => MyController<AssignmentsController>
                .Calling(c => c.Edit(10, With.Empty<AssignmentInputModel>()))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void EditPostWithValidInputModelAndIdShouldEditAssignment()
            => MyController<AssignmentsController>
                .Instance()
                .WithData(GetAssignment(architectId: "1"))
                .WithData(GetUser(designerType: UserType.ElectroEngineer))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Edit(1, new AssignmentInputModel() { BuildingId = 1, Description = new List<IFormFile>(), UserType = UserType.ElectroEngineer, EndDate = DateTime.Now + TimeSpan.FromDays(5) }))
                .ShouldHave()
                .Data(data => data.WithSet<Assignment>(set =>
                {
                    Assert.NotNull(set.SingleOrDefault(a => a.BuildingId == 1 && a.UserType == UserType.ElectroEngineer));
                }))
                .AndAlso()
                .ShouldReturn()
                .Redirect("/buildings/details/1");

        [Fact]
        public void DeleteShouldReturnNotFoundWhenIdIsInvalid()
            => MyController<AssignmentsController>
                .Calling(c => c.Delete(1))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void DeleteShouldReturnErrorViewWithCorrectModelAndExceptionWhenUserHasntCreatedAssignment()
            => MyController<AssignmentsController>
                .Instance()
                .WithData(GetAssignment())
                .WithData(GetUser(designerType: UserType.ElectroEngineer))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Delete(1))
                .ShouldReturn()
                .View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал заданието, може да го изтрие!" });

        [Fact]
        public void DeleteShouldDeleteAssignmentAndRedirectToHomePageWhenUserHasCreatedAssignment()
            => MyController<AssignmentsController>
                .Instance()
                .WithData(GetAssignment(architectId: ControllerConstants.UserId))
                .WithData(GetUser(designerType: UserType.ElectroEngineer))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Delete(1))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Assignment>(set =>
                    {
                        Assert.Equal(0, set.Count());
                    }))
                .AndAlso()
                .ShouldReturn()
                .Redirect("/buildings/details/1");

        private static IEnumerable<Bid> Get10UsersBidForAssignment(int assignmentId = 1)
        {
            var bids = new List<Bid>();

            for (int i = 0; i < 10; i++)
            {
                bids.Add(new Bid()
                {
                    AssignmentId = assignmentId,
                    DesignerId = (i + 2).ToString(),
                    Designer = new ApplicationUser() { Id = (i + 2).ToString() }
                });
            }

            return bids;
        }

        private static Assignment GetAssignment(int id = 1, UserType designerType = UserType.Other, string architectId = ControllerConstants.Architectid)
        {
            return new Assignment()
            {
                Id = id,
                Building = new Building() { ArchitectId = architectId },
                UserType = designerType,
            };
        }

        private static ApplicationUser GetUser(UserType designerType, string userId = ControllerConstants.UserId, string username = ControllerConstants.Username)
        {
            var user = new ApplicationUser() { UserType = designerType, Id = userId, UserName = username, };

            return user;
        }
    }
}
