namespace DesignAndBuilding.Web.ViewModels.Building
{
    using System.ComponentModel.DataAnnotations;

    public class BuildingInputModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "Името трябва да е между 2 и 50 символа!")]
        [MaxLength(50, ErrorMessage = "Името трябва да е между 2 и 50 символа!")]
        [Display(Name = "Име")]
        public string Name { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Името на града трябва да е между 3 и 20 символа!")]
        [MaxLength(20, ErrorMessage = "Името на града трябва да е между 3 и 20 символа!")]
        [Display(Name = "Град")]
        public string Town { get; set; }

        [Required]
        [Range(10, double.MaxValue, ErrorMessage = "РЗП трябва да е поне 10кв.м.")]
        [Display(Name = "РЗП")]
        public decimal TotalBuildUpArea { get; set; }

        [Required]
        [Display(Name = "Тип на сградата")]
        public string BuildingType { get; set; }
    }
}
