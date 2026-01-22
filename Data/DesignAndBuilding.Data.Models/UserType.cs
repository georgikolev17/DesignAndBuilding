namespace DesignAndBuilding.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum UserType
    {
        [Display(Name = "Друг", ShortName = "Друг")]
        Other = 0,

        [Display(Name = "Архитект", ShortName = "АРХ")]
        Architect = 1,

        [Display(Name = "Конструкции на сгради и съоръжения", ShortName = "КСС")]
        BuildingConstructionEngineer = 2,

        [Display(Name = "Електротехника, автоматика, съобщителна техника", ShortName = "ЕАСТ")]
        ElectroEngineer = 3,

        [Display(Name = "Водно строителство", ShortName = "ВС")]
        PlumbingEngineer = 4,

        [Display(Name = "Отопление, вентилация, климатизация, хладилна техника, топло- и газоснабдяване", ShortName = "ОВКХТТГ")]
        HVACEngineer = 5,

        [Display(Name = "Транспортно строителство и транспортни съоръжения", ShortName = "ТСТС")]
        TransportConstructionEngineer = 6,

        [Display(Name = "Геодезия и приложна геодезия", ShortName = "ГПГ")]
        GeodesyEngineer = 8,

        [Display(Name = "Минно дело, геология и екология", ShortName = "МДГЕ")]
        MiningGeologyEcologyEngineer = 9,

        [Display(Name = "Технологии", ShortName = "ТЕХ")]
        TechnologyEngineer = 10,
    }
}
