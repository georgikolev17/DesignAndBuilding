namespace DesignAndBuilding.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum BuildingType
    {
        [Display(Name = "Друг")]
        Other = 0,
        [Display(Name = "Хотел")]
        Hotel = 1,
        [Display(Name = "Детска градина")]
        Kindergarten = 2,
        [Display(Name = "Еднофамилна жилищна сграда")]
        SingleFamilyHouse = 3,
        [Display(Name = "Многофамилна жилищна сграда")]
        MultiFamiyHouse = 4,
        [Display(Name = "Болница")]
        Hospital = 5,
        [Display(Name = "Производствено предприятие")]
        Factory = 6,
        [Display(Name = "Административна сграда")]
        AdministrationBuilding = 7,
        [Display(Name = "Училище")]
        School = 8,
        [Display(Name = "Университет")]
        University = 9,
        [Display(Name = "Търговска сграда")]
        ComercialBuilding = 10,
        [Display(Name = "Спортна сграда")]
        SportBuilding = 11,
        [Display(Name = "Сграда за изкуство и култура")]
        ArtAndCultureBuilding = 12,
    }
}
