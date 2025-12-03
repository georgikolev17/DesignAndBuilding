namespace DesignAndBuilding.Web.ViewModels.Assignment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DesignAndBuilding.Data.Models;
    using Microsoft.AspNetCore.Http;

    public class AssignmentListViewModel
    {
        public int Id { get; set; }

        [Required]
        public string BuildingName { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public UserType UserType { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string ArchitectName { get; set; }

        public bool UserPlacedBid { get; set; }

        public decimal? UserBestBid { get; set; }

        public decimal? BestBid { get; set; }

        public bool IsActive => this.EndDate.Date >= DateTime.Now.Date;
    }
}
