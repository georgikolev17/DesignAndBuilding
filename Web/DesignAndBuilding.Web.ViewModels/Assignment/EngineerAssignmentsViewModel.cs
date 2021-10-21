namespace DesignAndBuilding.Web.ViewModels.Assignment
{
    using System.Collections.Generic;

    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.ViewModels.Building;

    public class EngineerAssignmentsViewModel
    {
        public EngineerAssignmentsViewModel()
        {
            this.FinishedAssignments = new List<BuildingDetailsAssignmentViewModel>();
            this.ActiveAssignments = new List<BuildingDetailsAssignmentViewModel>();
        }

        public IList<BuildingDetailsAssignmentViewModel> FinishedAssignments { get; set; }

        public IList<BuildingDetailsAssignmentViewModel> ActiveAssignments { get; set; }

        public DesignerType DesignerType { get; set; }

        public AssignmentSearchInputModel Search { get; set; }
    }
}
