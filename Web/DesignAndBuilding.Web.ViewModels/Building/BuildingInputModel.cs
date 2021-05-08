namespace DesignAndBuilding.Web.ViewModels.Building
{
    using System.ComponentModel.DataAnnotations;

    public class BuildingInputModel
    {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [Display(Name = "Име")]
        public string Name { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        [Display(Name = "Град")]
        public string Town { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "РЗП")]
        public decimal TotalBuildUpArea { get; set; }

        [Required]
        [Display(Name = "Тип на сградата")]
        public string BuildingType { get; set; }
    }
}
