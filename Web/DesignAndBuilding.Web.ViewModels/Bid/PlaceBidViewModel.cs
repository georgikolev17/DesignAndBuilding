namespace DesignAndBuilding.Web.ViewModels.Bid
{
    using System.ComponentModel.DataAnnotations;

    public class PlaceBidViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Цена лв/кв.м.")]
        [Range(0, 20)]
        public decimal BidPrice { get; set; }
    }
}
