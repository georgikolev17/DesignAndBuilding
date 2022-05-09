namespace DesignAndBuilding.Web.ViewModels.Assignment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DesignAndBuilding.Data.Models;
    using Microsoft.AspNetCore.Http;

    public class AssignmentViewModel
    {
        public AssignmentViewModel()
        {
            this.Bids = new HashSet<AssignmentBidViewModel>();
        }

        [Required]
        public int AssignmentId { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public UserType UserType { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public virtual AssignmentBuildingViewModel Building { get; set; }

        public virtual ICollection<AssignmentBidViewModel> Bids { get; set; }

        [Display(Name = "Цена лв/кв.м.")]
        public decimal BidPrice { get; set; }

        public bool IsFinished { get; set; }

        public bool HasUserCreatedAssignment { get; set; }
    }
}
