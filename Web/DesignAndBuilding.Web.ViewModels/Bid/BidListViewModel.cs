namespace DesignAndBuilding.Web.ViewModels.Bid
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class BidListViewModel
    {
        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime TimePlaced { get; set; }

        [Required]
        public string UserFullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}