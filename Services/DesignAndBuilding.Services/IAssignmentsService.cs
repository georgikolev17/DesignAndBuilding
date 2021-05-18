namespace DesignAndBuilding.Services
{
    using System;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Models;

    public interface IAssignmentsService
    {
        Task CreateAssignmentAsync(string description, DateTime endDate, DesignerType designerType, decimal basePricePerSquareMeter, int buildingId);
    }
}
