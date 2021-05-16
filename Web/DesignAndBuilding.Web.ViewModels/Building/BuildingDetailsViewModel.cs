namespace DesignAndBuilding.Web.ViewModels.Building
{
    using System.Collections.Generic;

    using DesignAndBuilding.Data.Models;

    public class BuildingDetailsViewModel
    {
        public string Name { get; set; }

        public decimal TotalBuildUpArea { get; set; }

        public BuildingType BuildingType { get; set; }

        public IEnumerable<BuildingDetailsAssignmentViewModel> Assignments { get; set; }
    }
}
