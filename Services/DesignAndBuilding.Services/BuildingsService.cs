namespace DesignAndBuilding.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using DesignAndBuilding.Data.Common.Repositories;
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.ViewModels.Building;
    using Microsoft.EntityFrameworkCore;

    public class BuildingsService : IBuildingsService
    {
        private readonly IDeletableEntityRepository<Building> buildingsRepository;
        private readonly IConfigurationProvider mapper;

        public BuildingsService(IDeletableEntityRepository<Building> buildingsRepository, IMapper mapper)
        {
            this.buildingsRepository = buildingsRepository;
            this.mapper = mapper.ConfigurationProvider;
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

        public async Task DeleteBuilding(int id)
        {
            var building = await this.buildingsRepository.All().FirstOrDefaultAsync(x => x.Id == id);

            building.IsDeleted = true;

            await this.buildingsRepository.SaveChangesAsync();
        }

        public async Task EditBuilding(int id, string buildingType, decimal totalBuildUpArea, string town, string name)
        {
            var building = await this.buildingsRepository.All().FirstOrDefaultAsync(x => x.Id == id);

            if (building == null)
            {
                return;
            }

            building.BuildingType = (BuildingType)Enum.Parse(typeof(BuildingType), buildingType);
            building.TotalBuildUpArea = totalBuildUpArea;
            building.Town = town;
            building.Name = name;

            await this.buildingsRepository.SaveChangesAsync();
        }

        public async Task<ICollection<Building>> GetAllBuildings()
        {
            var buildings = await this.buildingsRepository.All().ToListAsync();

            return buildings;
        }

        public IEnumerable<BuildingSummaryViewModel> GetAllBuildingsOfCurrentUserById(string id)
        {
            var buildings = this.buildingsRepository.All()
                .Where(x => x.ArchitectId == id)
                .ProjectTo<BuildingSummaryViewModel>(this.mapper)
                .ToList();

            return buildings;
        }

        public async Task<Building> GetBuildingById(int id)
        {
            var building = await this.buildingsRepository.All()
                .Include(x => x.Assignments)
                .FirstOrDefaultAsync(x => x.Id == id);
            return building;
        }

        public int GetBuildingsCount()
        {
            return this.buildingsRepository.All().Count();
        }

        public async Task<bool> HasUserCreatedBuilding(string userId, int buildingId)
        {
            var building = await this.buildingsRepository.All().FirstOrDefaultAsync(x => x.Id == buildingId);

            // Check if such building exists
            if (building == null)
            {
                return false;
            }

            return building.ArchitectId == userId;
        }
    }
}
