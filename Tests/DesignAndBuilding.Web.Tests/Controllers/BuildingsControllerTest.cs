namespace DesignAndBuilding.Web.Tests.Controllers
{
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.Controllers;
    using DesignAndBuilding.Web.ViewModels.Building;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class BuildingsControllerTest
    {
        [Fact]
        public void DetailsShouldReturnViewWithCorrectDataAndModel()
            => MyMvc
                .Pipeline()
                .ShouldMap("/buildings/details")
                .To<BuildingsController>(c => c.Details(4))
                .Which(/*controller => controller
                    .WithData(Get10AssignmentsForBuildingWithId4())*/)
                /*.ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests()
                    .ValidatingAntiForgeryToken())
                .AndAlso()*/
                .ShouldReturn()
                .View(/*view => view
                    .WithModelOfType<BuildingDetailsViewModel>()
                    .Passing(m => Assert.Equal(10, m.Assignments.Count()))*/);

        private static IEnumerable<Assignment> Get10AssignmentsForBuildingWithId4()
        {
            return Enumerable.Range(0, 10).Select(x => new Assignment() { BuildingId = 4 });
        }
    }
}
