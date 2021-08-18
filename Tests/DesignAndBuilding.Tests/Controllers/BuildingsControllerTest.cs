namespace DesignAndBuilding.Tests.Controllers
{
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.Controllers;
    using DesignAndBuilding.Web.ViewModels;
    using DesignAndBuilding.Web.ViewModels.Building;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class BuildingsControllerTest
    {
        [Fact]
        public void ControllerShouldHaveAuthorizeAttribute()
            => MyController<BuildingsController>
                .ShouldHave()
                .Attributes(a => a.RestrictingForAuthorizedRequests());

        //Create - GET

        [Fact]
        public void CreateGetShouldReturnView()
            => MyController<BuildingsController>
                .Instance()
                .WithData(GetUser(designerType: DesignerType.Architect))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Create())
                .ShouldReturn()
                .View();

        [Fact]
        public void CreateGetShouldReturnErrorViewWhenTriedToBeAccessedByUserWhoIsNotArchitect()
            => MyController<BuildingsController>
                .Instance()
                .WithData(GetUser(designerType: DesignerType.ElectroEngineer))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Create())
                .ShouldReturn()
                .View("Error", new ErrorViewModel() { ErrorMessage = "Само архитекти могат да създават обекти!" });

        //Create - POST
        [Fact]
        public void CreatePostShouldHavePostAttribute()
            => MyController<BuildingsController>
                .Instance()
                .WithData(GetUser(designerType: DesignerType.ElectroEngineer))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Create(With.Empty<BuildingInputModel>()))
                .ShouldHave()
                .ActionAttributes(a => a.RestrictingForHttpMethod(HttpMethod.Post));

        [Fact]
        public void CreatePostShouldReturnViewWithCorrectModelWhenModelStateIsInvalid()
            => MyController<BuildingsController>
                .Instance()
                .WithData(GetUser(designerType: DesignerType.Architect))
                .WithUser(user => 
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Create(With.Default<BuildingInputModel>()))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<BuildingInputModel>());

        [Fact]
        public void CreatePostShouldReturnErrorViewWhenTriedToBeAccessedByUserWhoIsNotArchitect()
            => MyController<BuildingsController>
                .Instance()
                .WithData(GetUser(designerType: DesignerType.ElectroEngineer))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Create(new BuildingInputModel() { BuildingType = "test", Name = "test", TotalBuildUpArea = 200, Town = "test" }))
                .ShouldReturn()
                .View("Error", new ErrorViewModel() { ErrorMessage = "Само архитекти могат да създават обекти!" });

        [Fact]
        public void CreatePostShouldCreateNewBuildingWhenModelStateIsValidAndUserIsArchitect()
            => MyController<BuildingsController>
                .Instance()
                .WithData(GetUser(designerType: DesignerType.Architect))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Create(new BuildingInputModel() { BuildingType = BuildingType.Hotel.ToString(), Name = "test", TotalBuildUpArea = 200, Town = "test" }))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Building>(set =>
                    {
                        Assert.Equal(1, set.Count());
                        Assert.NotNull(set.SingleOrDefault(b => b.Name == "test" && b.BuildingType == BuildingType.Hotel && b.TotalBuildUpArea == 200 && b.Town == "test"));
                    }))
                .AndAlso()
                .ShouldReturn()
                .Redirect("mybuildings");

        //MyBuildings

        [Fact]
        public void MyBuildingsShouldReturnErrorViewWhenUserIsNotArchitect()
            => MyController<BuildingsController>
                .Instance()
                .WithData(GetUser(designerType: DesignerType.ElectroEngineer))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.MyBuildings())
                .ShouldReturn()
                .View("Error", new ErrorViewModel() { ErrorMessage = "Само архитекти могат да достъпват тази страница!" });

        [Fact]
        public void myBuildingsShouldReturnViewWithCorrectModelAndDataWhenUserIsArchietct()
            => MyController<BuildingsController>
                .Instance()
                .WithData(GetUser(designerType: DesignerType.Architect))
                .WithData(Get10BuildingsForArchitect())
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.MyBuildings())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<IEnumerable<MyBuildingsViewModel>>()
                    .Passing(x => Assert.Equal(10, x.Count())));

        //Details

        [Fact]
        public void DetailsShouldReturnNotFoundWhenIdIsInvalid()
            => MyController<BuildingsController>
                .Instance()
                .Calling(c => c.Details(1))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void DetailsShouldReturnErrorViewWithCorrectModelAndErrorMessageWhenUserHasntCreatedBuilding()
            => MyController<BuildingsController>
                .Instance()
                .WithData(Get10BuildingsForArchitect("2"))
                .WithData(GetUser(designerType: DesignerType.Architect))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Details(1))
                .ShouldReturn()
                .View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал проекта, може да вижда детейлите му!" });

        [Fact]
        public void DetailsShouldReturnViewWithCorrectModelAndData()
            => MyController<BuildingsController>
                .Instance()
                .WithData(Get1BuildingDetails())
                .WithData(GetUser(designerType: DesignerType.Architect))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Details(1))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<BuildingDetailsViewModel>()
                    .Passing(x => x.Name == "Test name" && x.TotalBuildUpArea == 100 && x.BuildingType == BuildingType.Other && x.Assignments.Count() == 0));

        // Edit - GET
        [Fact]
        public void EditGetShouldReturnNotFoundWhenIdIsInvalid()
            => MyController<BuildingsController>
                .Instance()
                .Calling(c => c.Edit(1))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void EditGetShouldReturnErrorViewWithCorrectModelAndErrorMessageWhenUserHasntCreatedBuilding()
            => MyController<BuildingsController>
                .Instance()
                .WithData(Get10BuildingsForArchitect("2"))
                .WithData(GetUser(designerType: DesignerType.Architect))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Edit(1))
                .ShouldReturn()
                .View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал проекта, може да го редактира!" });


        [Fact]
        public void EditGetShouldReturnViewWithCorrectModelAndData()
            => MyController<BuildingsController>
                .Instance()
                .WithData(GetUser(designerType: DesignerType.Architect))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .WithData(Get1BuildingDetails())
                .Calling(c => c.Edit(1))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<BuildingInputModel>()
                    .Passing(x => x.Name == "Test name" && x.Town == "Test town" && x.BuildingType == BuildingType.Other.ToString()));

        // Edit - POST

        [Fact]
        public void EditPostShouldHavePostAttribute()
            => MyController<BuildingsController>
                .Calling(c => c.Edit(1, With.Default<BuildingInputModel>()))
                .ShouldHave()
                .ActionAttributes(a => a.RestrictingForHttpMethod(HttpMethod.Post));

        [Fact]
        public void EditPostShouldReturnNotFoundWhenIdIsInvalid()
            => MyController<BuildingsController>
                .Instance()
                .Calling(c => c.Edit(1, With.Empty<BuildingInputModel>()))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void EditPostShouldReturnErrorViewWithCorrectModelAndErrorMessageWhenUserHasntCreatedBuilding()
            => MyController<BuildingsController>
                .Instance()
                .WithData(Get10BuildingsForArchitect("2"))
                .WithData(GetUser(designerType: DesignerType.Architect))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Edit(1, With.Default<BuildingInputModel>()))
                .ShouldReturn()
                .View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал проекта, може да го редактира!" });

        [Fact]
        public void EditPostShouldEditBuildingAndRedirectToMyBuildings()
            => MyController<BuildingsController>
                .Instance()
                .WithData(GetUser(designerType: DesignerType.Architect))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .WithData(Get1BuildingDetails())
                .Calling(c => c.Edit(1, new BuildingInputModel() { BuildingType = BuildingType.Hotel.ToString(), Name = "Changed name", TotalBuildUpArea = 150, Town = "Changed town" }))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Building>(set =>
                    {
                        Assert.NotNull(set.SingleOrDefault(b => b.Name == "Changed name" && b.TotalBuildUpArea == 150));
                    }))
                .AndAlso()
                .ShouldReturn()
                .Redirect("/buildings/mybuildings");


        //Delete

        [Fact]
        public void DeleteShouldReturnNotFoundWhenIdIsInvalid()
            => MyController<BuildingsController>
                .Instance()
                .Calling(c => c.Delete(1))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void DeleteShouldReturnErrorViewWithCorrectModelAndErrorMessageWhenUserHasntCreatedBuilding()
            => MyController<BuildingsController>
                .Instance()
                .WithData(Get10BuildingsForArchitect("2"))
                .WithData(GetUser(designerType: DesignerType.Architect))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .Calling(c => c.Delete(1))
                .ShouldReturn()
                .View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал проекта, може да го изтрие!" });

        [Fact]
        public void DeleteShouldDeleteBuildingAndRedirectToMyBuildings()
            => MyController<BuildingsController>
                .Instance()
                .WithData(GetUser(designerType: DesignerType.Architect))
                .WithUser(user =>
                {
                    user.WithIdentifier(ControllerConstants.UserId);
                    user.WithUsername(ControllerConstants.Username);
                })
                .WithData(Get1BuildingDetails())
                .Calling(c => c.Delete(1))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Building>(set =>
                    {
                        Assert.Equal(0, set.Count());
                    }))
                .AndAlso()
                .ShouldReturn()
                .Redirect("/buildings/mybuildings");

        //Static methods

        private static ApplicationUser GetUser(DesignerType designerType, string userId = ControllerConstants.UserId, string username = ControllerConstants.Username)
        {
            var user = new ApplicationUser() { DesignerType = designerType, Id = userId, UserName = username, };

            return user;
        }

        private static IEnumerable<Building> Get10BuildingsForArchitect(string userId = ControllerConstants.UserId)
            => Enumerable.Range(0, 10).Select(b => new Building() { ArchitectId = userId });

        private static Building Get1BuildingDetails()
            => new Building()
            {
                ArchitectId = ControllerConstants.UserId,
                BuildingType = BuildingType.Other,
                Name = "Test name",
                Town = "Test town",
                TotalBuildUpArea = 100,
            };
    }
}
