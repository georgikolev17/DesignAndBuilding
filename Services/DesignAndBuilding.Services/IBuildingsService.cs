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
    }
}