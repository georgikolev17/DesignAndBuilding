namespace DesignAndBuilding.Web.ViewModels.Bid
{
    using System.ComponentModel.DataAnnotations;

    public class PlaceBidViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Цена лв/кв.м.")]
        [RegularExpression(@"^[-+]?[0-9]*\,?[0-9]+([eE][-+]?[0-9]+)?$")]
        public string BidPrice { get; set; }
    }
}
