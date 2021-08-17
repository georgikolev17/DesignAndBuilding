namespace DesignAndBuilding.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.ViewModels.Building;

    public interface IBuildingsService
    {
        Task<int> CreateBuildingAsync(string architectId, string name, string town, decimal totalBuildUpArea, string buildingType);

        IEnumerable<MyBuildingsViewModel> GetAllBuildingsOfCurrentUserById(string id);

        Task<Building> GetBuildingById(int id);

        Task DeleteBuilding(int id);

        Task EditBuilding(int id, string buildingType, decimal totalBuildUpArea, string town, string name);

        Task<bool> HasUserCreatedBuilding(string userId, int buildingId);

        int GetBuildingsCount();
    }
}