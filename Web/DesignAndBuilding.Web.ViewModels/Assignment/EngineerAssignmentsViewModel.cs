namespace DesignAndBuilding.Web.ViewModels.Assignment
{
    using System.Collections.Generic;

    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.ViewModels.Building;

    public class EngineerAssignmentsViewModel
    {
        public EngineerAssignmentsViewModel()
        {
            this.Assignments = new List<BuildingDetailsAssignmentViewModel>();
        }

        public IList<BuildingDetailsAssignmentViewModel> Assignments { get; set; }

        public DesignerType DesignerType { get; set; }
    }
}
