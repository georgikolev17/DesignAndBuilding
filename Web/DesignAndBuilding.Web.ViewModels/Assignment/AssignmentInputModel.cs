namespace DesignAndBuilding.Web.ViewModels.Assignment
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DesignAndBuilding.Data.Models;

    public class AssignmentInputModel
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public DesignerType DesignerType { get; set; }

        [Required]
        public int BuildingId { get; set; }
    }
}
