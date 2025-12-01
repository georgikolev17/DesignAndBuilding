namespace DesignAndBuilding.Web.ViewModels.Building
{
    using System.Collections.Generic;

    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.ViewModels.Assignment;

    public class BuildingDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal TotalBuildUpArea { get; set; }

        public BuildingType BuildingType { get; set; }

        public IList<AssignmentSummaryViewModel> Assignments { get; set; }
    }
}
