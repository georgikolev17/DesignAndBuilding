namespace DesignAndBuilding.Web.ViewModels.Assignment
{
    using DesignAndBuilding.Data.Models;

    public class AssignmentBuildingViewModel
    {
        public string Name { get; set; }

        public decimal TotalBuildUpArea { get; set; }

        public BuildingType BuildingType { get; set; }
    }
}