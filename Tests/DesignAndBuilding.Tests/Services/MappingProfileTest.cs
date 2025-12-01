using System;
using System.Collections.Generic;
using AutoMapper;
using DesignAndBuilding.Data.Models;
using DesignAndBuilding.Services.Mapping;
using DesignAndBuilding.Web.ViewModels.Assignment;
using Xunit;

namespace DesignAndBuilding.Tests.Services
{
    public class MappingTests
    {
        private readonly IMapper mapper;

        public MappingTests()
        {
            // 1. Setup the Configuration Provider with your Profile
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            this.mapper = config.CreateMapper();
        }

        [Fact]
        public void AssignmentToBuildingDetailsAssignmentViewModel_ShouldMapContextDataCorrectly()
        {
            // Arrange
            var targetUserId = "user-123";
            var otherUserId = "user-456";
            var architectId = "arch-789";

            var assignment = new Assignment
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
                UserType = UserType.ElectroEngineer,
                Building = new Building
                {
                    Name = "Sky Scraper 1",
                    Architect = new ApplicationUser
                    {
                        FirstName = "Ivan",
                        LastName = "Ivanov",
                    }
                },
                Bids = new List<Bid>
                {
                    // Target user's bids
                    new Bid { DesignerId = targetUserId, Price = 5000 },
                    new Bid { DesignerId = targetUserId, Price = 4500 }, // User's best bid
                    
                    // Other user's bids (better overall)
                    new Bid { DesignerId = otherUserId, Price = 4000 },  // Global best bid
                    new Bid { DesignerId = otherUserId, Price = 6000 }
                }
            };

            // Act
            // Pass the custom UserId via the Items dictionary
            var viewModel = this.mapper.Map<AssignmentListViewModel>(
                assignment,
                opts => opts.Items["UserId"] = targetUserId
            );

            // Assert
            Assert.NotNull(viewModel);

            // 1. Check Standard Properties
            Assert.Equal("Sky Scraper 1", viewModel.BuildingName);
            Assert.Equal("инж. Ivan Ivanov", viewModel.ArchitectName);
            Assert.Equal(UserType.ElectroEngineer, viewModel.UserType);

            // 2. Check Context-Specific Logic
            Assert.True(viewModel.UserPlacedBid, "UserPlacedBid should be true because targetUserId exists in Bids list");

            // 3. Check Calculated Aggregates
            Assert.Equal(4000, viewModel.BestBid); // Should be the lowest of ALL bids
            Assert.Equal(4500, viewModel.UserBestBid); // Should be the lowest of TARGET USER's bids
        }

        [Fact]
        public void AssignmentToBuildingDetailsAssignmentViewModel_ShouldHandleNoBidsFromUser()
        {
            // Arrange
            var targetUserId = "user-123";
            var otherUserId = "user-456";

            var assignment = new Assignment
            {
                Id = 2,
                Building = new Building { Name = "Small House", Architect = new ApplicationUser { FirstName = "Test", LastName = "Testov" } },
                Bids = new List<Bid>
                {
                    new Bid { DesignerId = otherUserId, Price = 3000 }
                }
            };

            // Act
            var viewModel = this.mapper.Map<AssignmentListViewModel>(
                assignment,
                opts => opts.Items["UserId"] = targetUserId
            );

            // Assert
            Assert.False(viewModel.UserPlacedBid);
            Assert.Null(viewModel.UserBestBid);
            Assert.Equal(3000, viewModel.BestBid);
        }
    }
}