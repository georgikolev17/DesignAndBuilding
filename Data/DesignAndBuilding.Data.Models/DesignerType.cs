namespace DesignAndBuilding.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum DesignerType
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
    }
}
