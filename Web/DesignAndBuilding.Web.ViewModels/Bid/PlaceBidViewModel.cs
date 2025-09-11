namespace DesignAndBuilding.Web.ViewModels.Bid
{
    using System.ComponentModel.DataAnnotations;

    public class PlaceBidViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Цена лв/кв.м.")]
        [Range(0, int.MaxValue, ErrorMessage = "Предлаганата цена трябва да е по-голяма от 0 лв./кв.м.")]
        public decimal BidPrice { get; set; }
    }
}
