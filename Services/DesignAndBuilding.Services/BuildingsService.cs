namespace DesignAndBuilding.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data;
    using DesignAndBuilding.Data.Common.Repositories;
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.ViewModels.Building;

    public class BuildingsService : IBuildingsService
    {
        private readonly IDeletableEntityRepository<Building> buildingsRepository;

        public BuildingsService(IDeletableEntityRepository<Building> buildingsRepository)
        {
            this.buildingsRepository = buildingsRepository;
        }

        public async Task<int> CreateBuildingAsync(string architectId, string name, string town, decimal totalBuildUpArea, string buildingType)
        {
            var building = new Building()
            {
                ArchitectId = architectId,
                Name = name,
                TotalBuildUpArea = totalBuildUpArea,
                BuildingType = (BuildingType)Enum.Parse(typeof(BuildingType), buildingType),
                Town = town,
            };

            await this.buildingsRepository.AddAsync(building);
            await this.buildingsRepository.SaveChangesAsync();

            return building.Id;
        }

        public IEnumerable<MyBuildingsViewModel> GetAllBuildingsOfCurrentUserById(string id)
        {
            var buildings = this.buildingsRepository.All()
                .Where(x => x.ArchitectId == id)
                .Select(x => new MyBuildingsViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    BuildingType = x.BuildingType,
                    TotalBuildUpArea = x.TotalBuildUpArea,
                })
                .ToList();

            return buildings;
        }

        public BuildingDetailsViewModel GetBuildingById(int id)
        {
            var building = this.buildingsRepository
                .All()
                .Where(x => x.Id == id)
                .Select(x => new BuildingDetailsViewModel
                {
                    Id = id,
                    Name = x.Name,
                    BuildingType = x.BuildingType,
                    TotalBuildUpArea = x.TotalBuildUpArea,
                    Assignments = x.Assignments.Select(a => new BuildingDetailsAssignmentViewModel
                    {
                        Id = a.Id,
                        BasePricePerSquareMeter = a.BasePricePerSquareMeter,
                        Description = a.Description,
                        DesignerType = a.DesignerType,
                        EndDate = a.EndDate,
                    }),
                }).FirstOrDefault();

            return building;
        }
    }
}
