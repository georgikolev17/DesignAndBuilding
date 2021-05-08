namespace DesignAndBuilding.Services
{
    using DesignAndBuilding.Web.ViewModels.Building;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBuildingsService
    {
        Task<int> CreateBuildingAsync(string architectId, string name, string town, decimal totalBuildUpArea, string buildingType);

        IEnumerable<MyBuildingsViewModel> GetAllBuildingsOfCurrentUserById(string id);
    }
}