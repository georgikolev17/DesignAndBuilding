namespace DesignAndBuilding.Web.ViewModels.Bid
{
    using System.ComponentModel.DataAnnotations;

    public class PlaceBidViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Цена лв/кв.м.")]
        public string BidPrice { get; set; }
    }
}
