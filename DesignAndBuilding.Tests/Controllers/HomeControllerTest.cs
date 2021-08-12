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

        [Theory]
        [InlineData(DesignerType.ElectroEngineer, 10)]
        // Default user is Electro engineer so he shouldn't see other engineer's assignments
        [InlineData(DesignerType.BuildingConstructionEngineer, 0)]
        [InlineData(DesignerType.PlumbingEngineer, 0)]
        [InlineData(DesignerType.HVACEngineer, 0)]
        [InlineData(DesignerType.Other, 0)]
        public void IndexShouldReturnCorrectViewWithCoreectDataAndModel(DesignerType designerType, int expectedAssignmentsCount)
        {
            MyController<HomeController>
               .Instance()
                .WithData(Get10Assignments(designerType))
                .WithData(GetUser(DesignerType.Architect))
               .Calling(c => c.Index())
               .ShouldReturn()
               .View(view => view
                   .WithModelOfType<EngineerAssignmentsViewModel>()
                   .Passing(m => Assert.Equal(expectedAssignmentsCount, m.Assignments.Count)));
        }

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

        private static ApplicationUser GetUser(DesignerType designerType)
        {
            return new ApplicationUser()
            {
                Id = ControllerConstants.UserId,
                FirstName = ControllerConstants.UserFirstName,
                LastName = ControllerConstants.UserLastName,
                DesignerType = designerType,
            };
        }
    }
}
