namespace DesignAndBuilding.Web.ViewModels.Assignment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DesignAndBuilding.Data.Models;

    public class AssignmentViewModel
    {
        public AssignmentViewModel()
        {
            this.Bids = new HashSet<AssignmentBidViewModel>();
        }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public DesignerType DesignerType { get; set; }

        [Required]
        public decimal BasePricePerSquareMeter { get; set; }

        public virtual AssignmentBuildingViewModel Building { get; set; }

        public virtual ICollection<AssignmentBidViewModel> Bids { get; set; }

        public string BidPrice { get; set; }
    }
}
