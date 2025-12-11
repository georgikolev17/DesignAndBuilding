namespace DesignAndBuilding.Web.ViewModels.Assignment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.ViewModels.Bid;
    using DesignAndBuilding.Web.ViewModels.Building;
    using DesignAndBuilding.Web.ViewModels.Question;
    using Microsoft.AspNetCore.Http;

    public class AssignmentDetailsViewModel
    {
        public AssignmentDetailsViewModel()
        {
            this.Bids = new HashSet<BidListViewModel>();
            this.Questions = new HashSet<QuestionListViewModel>();
        }

        [Required]
        public int AssignmentId { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public UserType UserType { get; set; }

        public string ArchitectName { get; set; }

        public string DescriptionText { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public virtual BuildingSummaryViewModel Building { get; set; }

        public virtual ICollection<BidListViewModel> Bids { get; set; }

        public virtual ICollection<QuestionListViewModel> Questions { get; set; }

        [Required(ErrorMessage = "Въведете цена, за да може да предложите нова оферта")]
        [Display(Name = "Цена лв/кв.м.")]
        public decimal BidPrice { get; set; }

        public bool IsFinished => DateTime.UtcNow.Date > this.EndDate.Date;

        public bool HasUserCreatedAssignment { get; set; }
    }
}
