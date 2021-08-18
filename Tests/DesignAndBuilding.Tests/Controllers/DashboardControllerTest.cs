using DesignAndBuilding.Common;
using DesignAndBuilding.Data.Models;
using DesignAndBuilding.Web.Areas.Administration.Controllers;
using DesignAndBuilding.Web.ViewModels.Administration.Dashboard;
using DesignAndBuilding.Web.ViewModels.Building;
using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DesignAndBuilding.Tests.Controllers
{
    public class DashboardControllerTest
    {
        [Fact]
        public void ControllerShouldHaveAuthorizeAndAreaAttributes()
            => MyController<DashboardController>
                .ShouldHave()
                .Attributes(a =>
                {
                    a.RestrictingForAuthorizedRequests(GlobalConstants.AdministratorRoleName);
                    a.SpecifyingArea("Administration");
                });

        [Fact]
        public void IndexShouldReturnViewWithCorrectApplicationStatistics()
            => MyController<DashboardController>
                .Instance()
                .WithUser(user => user.WithUsername("admin@dab.com"))
                .WithData(Get10Users())
                .WithData(Get10Assignments())
                .WithData(Get10Buildings())
                .Calling(c => c.Index())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<IndexViewModel>()
                    .Passing(x =>
                    {
                        Assert.Equal(10, x.UsersCount);
                        Assert.Equal(10, x.AssignmentsCount);
                        Assert.Equal(10, x.BuildingsCount);
                    }));

        [Fact]
        public void AllBuildingsShouldReturnViewWithCorrectModelAndData()
            => MyController<DashboardController>
                .Instance()
                .WithData(Get10Buildings())
                .Calling(c => c.AllBuildings())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<List<MyBuildingsViewModel>>()
                    .Passing(x => x.Count == 10));

        private static IEnumerable<ApplicationUser> Get10Users()
            => Enumerable.Range(0, 10).Select(x => new ApplicationUser());

        private static IEnumerable<Building> Get10Buildings()
            => Enumerable.Range(0, 10).Select(x => new Building());

        private static IEnumerable<Assignment> Get10Assignments()
            => Enumerable.Range(0, 10).Select(x => new Assignment());
    }
}
