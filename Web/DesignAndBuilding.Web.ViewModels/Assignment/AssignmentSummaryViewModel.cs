namespace DesignAndBuilding.Web.ViewModels.Assignment
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DesignAndBuilding.Data.Models;

    public class AssignmentSummaryViewModel
    {
        public int Id { get; set; }

        public UserType UserType { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsFinished => DateTime.UtcNow.Date > this.EndDate.Date;
    }
}
