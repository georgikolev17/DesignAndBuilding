namespace DesignAndBuilding.Web.ViewModels.Bid
{
    using System.ComponentModel.DataAnnotations;

    public class PlaceBidViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Цена лв/кв.м.")]
        [Range(1, int.MaxValue, ErrorMessage = "Предлаганата цена трябва да е поне 1 лв./кв.м.")]
        public decimal BidPrice { get; set; }
    }
}
