namespace DesignAndBuilding.Web.ViewModels.Assignment
{
    using System;

    using DesignAndBuilding.Data.Models;

    public class AssignmentSearchInputModel
    {
        public string SearchText { get; set; } = null;

        public BuildingType? BuildingType { get; set; } = null;
    }
}
