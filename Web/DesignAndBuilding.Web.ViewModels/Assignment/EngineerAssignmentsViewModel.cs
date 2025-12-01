namespace DesignAndBuilding.Web.ViewModels.Assignment
{
    using System.Collections.Generic;

    using DesignAndBuilding.Data.Models;

    public class EngineerAssignmentsViewModel
    {
        public EngineerAssignmentsViewModel()
        {
            this.Assignments = new List<AssignmentListViewModel>();
        }

        public IList<AssignmentListViewModel> Assignments { get; set; }

        public UserType UserType { get; set; }
    }
}
