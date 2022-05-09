namespace DesignAndBuilding.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum UserType
    {
        [Display(Name = "Друг")]
        Other = 0,
        [Display(Name = "Архитект")]
        Architect = 1,
        [Display(Name = "Строителни конструкции")]
        BuildingConstructionEngineer = 2,
        [Display(Name = "Електро инженер")]
        ElectroEngineer = 3,
        [Display(Name = "ВиК инженер")]
        PlumbingEngineer = 4,
        [Display(Name = "ОВК инженер")]
        HVACEngineer = 5,
        [Display(Name = "Строителна компания")]
        BuildingCompany = 6,
        [Display(Name = "Инвестиционна компания")]
        InvestmentCompany = 7,
        [Display(Name = "Инвеститор (физическо лице)")]
        InvestmentPerson = 8,
    }
}
