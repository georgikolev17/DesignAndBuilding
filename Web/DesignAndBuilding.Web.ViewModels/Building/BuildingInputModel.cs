namespace DesignAndBuilding.Web.ViewModels.Building
{
    using System.ComponentModel.DataAnnotations;

    public class BuildingInputModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "Name should be at least 2 characters long!")]
        [MaxLength(50, ErrorMessage = "Name should be at max 50 characters long!")]
        [Display(Name = "Име")]
        public string Name { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Town name should be between 3 and 20 characters long!")]
        [MaxLength(20, ErrorMessage = "Town name should be between 3 and 20 characters long!")]
        [Display(Name = "Град")]
        public string Town { get; set; }

        [Required]
        [Range(10, double.MaxValue, ErrorMessage = "Total build-up area should be bigger than 10!")]
        [Display(Name = "РЗП")]
        public decimal TotalBuildUpArea { get; set; }

        [Required]
        [Display(Name = "Тип на сградата")]
        public string BuildingType { get; set; }
    }
}
