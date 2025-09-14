namespace DesignAndBuilding.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum UserType
    {
        [Display(Name = "Друг", ShortName = "Друг")]
        Other = 0,
        [Display(Name = "Архитект", ShortName = "АРХ")]
        Architect = 1,
        [Display(Name = "Строителни конструкции", ShortName = "СК")]
        BuildingConstructionEngineer = 2,
        [Display(Name = "Електро инженер", ShortName = "ЕЛ")]
        ElectroEngineer = 3,
        [Display(Name = "ВиК инженер", ShortName = "ВиК")]
        PlumbingEngineer = 4,
        [Display(Name = "ОВК инженер", ShortName = "ОВК")]
        HVACEngineer = 5,
        //[Display(Name = "Строителна компания", ShortName = "Строителна компания")]
        //BuildingCompany = 6,
        //[Display(Name = "Инвестиционна компания", ShortName = "Инвестиционна компания")]
        //InvestmentCompany = 7,
        //[Display(Name = "Инвеститор (физическо лице)", ShortName = "Инвеститор (физическо лице)")]
        //InvestmentPerson = 8,
    }
}
