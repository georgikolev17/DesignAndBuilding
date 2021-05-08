namespace DesignAndBuilding.Web.ViewModels.Building
{
    using DesignAndBuilding.Data.Models;

    public class MyBuildingsViewModel
    {
        public string Name { get; set; }

        public decimal TotalBuildUpArea { get; set; }

        public BuildingType BuildingType { get; set; }
    }
}
