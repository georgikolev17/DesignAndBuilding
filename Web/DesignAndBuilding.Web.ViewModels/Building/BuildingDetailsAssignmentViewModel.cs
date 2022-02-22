namespace DesignAndBuilding.Web.ViewModels.Building
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DesignAndBuilding.Data.Models;
    using Microsoft.AspNetCore.Http;

    public class BuildingDetailsAssignmentViewModel
    {
        public int Id { get; set; }

        [Required]
        public BuildingType BuildingType { get; set; }

        [Required]
        public ICollection<DescriptionFile> Description { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public DesignerType DesignerType { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string ArchitectName { get; set; }

        public bool UserPlacedBid { get; set; }

        public decimal? UserBestBid { get; set; }

        public decimal? BestBid { get; set; }
    }
}
