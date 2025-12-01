namespace DesignAndBuilding.Web.ViewModels.Building
{
    using DesignAndBuilding.Data.Models;

    public class BuildingSummaryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal TotalBuildUpArea { get; set; }

        public BuildingType BuildingType { get; set; }
    }
}
