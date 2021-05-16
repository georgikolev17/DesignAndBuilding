namespace DesignAndBuilding.Web.ViewModels.Building
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DesignAndBuilding.Data.Models;

    public class BuildingDetailsAssignmentViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public DesignerType DesignerType { get; set; }

        [Required]
        public decimal BasePricePerSquareMeter { get; set; }
    }
}