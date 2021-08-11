namespace DesignAndBuilding.Web.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.Controllers;
    using DesignAndBuilding.Web.ViewModels.Assignment;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModelAndData()
            => MyMvc
                .Pipeline()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index())
                .Which(controller => controller
                    .WithData(Get10ArchitectAssignments())
                    .WithData(new ApplicationUser() { DesignerType = DesignerType.Architect }))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<EngineerAssignmentsViewModel>()
                    .Passing(m => Assert.Equal(10, m.Assignments.Count)));

        private static IEnumerable<Assignment> Get10ArchitectAssignments()
            => Enumerable.Range(0, 10).Select(a => new Assignment() { DesignerType = DesignerType.Architect });
    }
}
