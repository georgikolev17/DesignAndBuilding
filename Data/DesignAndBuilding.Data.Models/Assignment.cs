﻿namespace DesignAndBuilding.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DesignAndBuilding.Data.Common.Models;
    using Microsoft.AspNetCore.Http;

    public class Assignment : BaseDeletableModel<int>
    {
        public Assignment()
        {
            this.Bids = new HashSet<Bid>();
            this.Description = new HashSet<DescriptionFile>();
        }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public UserType UserType { get; set; }

        [Required]
        public AssignmentType AssignmentType { get; set; }

        public bool IsFinished => this.EndDate < DateTime.Now;

        [Required]
        public int BuildingId { get; set; }

        public virtual Building Building { get; set; }

        public virtual ICollection<Bid> Bids { get; set; }

        public virtual ICollection<DescriptionFile> Description { get; set; }
    }
}
