namespace DesignAndBuilding.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DesignAndBuilding.Data.Common.Models;

    public class Assignment : BaseDeletableModel<int>
    {
        public Assignment()
        {
            this.Bids = new HashSet<Bid>();
        }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public DesignerType DesignerType { get; set; }

        public bool IsFinished => this.EndDate < DateTime.Now;

        [Required]
        public int BuildingId { get; set; }

        public virtual Building Building { get; set; }

        public virtual ICollection<Bid> Bids { get; set; }
    }
}
