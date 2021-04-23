namespace DesignAndBuilding.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DesignAndBuilding.Data.Common.Models;

    public class Bid : BaseDeletableModel<int>
    {
        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime TimePlaced { get; set; }

        [Required]
        public int AssignmentId { get; set; }

        public Assignment Assignment { get; set; }

        [Required]
        public int DesignerId { get; set; }

        public ApplicationUser Designer { get; set; }
    }
}
