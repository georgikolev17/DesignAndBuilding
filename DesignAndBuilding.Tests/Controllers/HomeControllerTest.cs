namespace DesignAndBuilding.Tests.Controllers
{
    using Xunit;
    using DesignAndBuilding.Web.Controllers;
    using MyTested.AspNetCore.Mvc;
    using DesignAndBuilding.Data.Models;
    using System.Collections.Generic;
    using System.Linq;
    using DesignAndBuilding.Web.ViewModels.Assignment;
    using Moq;
    using Microsoft.AspNetCore.Identity;
    using DesignAndBuilding.Tests.Mocks;
    using Microsoft.Extensions.Options;
    using System;
    using Microsoft.Extensions.Logging;

    public class HomeControllerTest
    {
        [Fact]
        public void PrivacyShouldReturnView()
            => MyController<HomeController>
                .Instance()
                .Calling(c => c.Privacy())
                .ShouldReturn()
                .View();

        [Fact]
        public void IndexShouldReturnCorrectViewWithCoreectDataAndModel()
        {
            MyController<HomeController>
               .Instance()
               .WithUser("TestUser")
               //.WithData(Get10ElectroAssignments())
               .Calling(c => c.Index())
               .ShouldReturn()
               .View(view => view
                   .WithModelOfType<EngineerAssignmentsViewModel>()
                   /*.Passing(m => Assert.Equal(10, m.Assignments.Count))*/);
        }

        /*private static IEnumerable<Assignment> Get10ElectroAssignments()
            => Enumerable.Range(0, 10).Select(a => new Assignment() 
            *//*{
                BuildingName = "ime",
                CreatedOn = DateTime.UtcNow,
                ArchitectName = "Ivan",
                Description = "Ivan",
                DesignerType = DesignerType.ElectroEngineer,
                EndDate = DateTime.UtcNow,
                Id = 1,
            }*//*);*/
    }
}
