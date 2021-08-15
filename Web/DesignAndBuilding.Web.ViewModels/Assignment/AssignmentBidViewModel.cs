namespace DesignAndBuilding.Web.ViewModels.Assignment
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AssignmentBidViewModel
    {
        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime TimePlaced { get; set; }

        [Required]
        public string UserFullName { get; set; }
    }
}