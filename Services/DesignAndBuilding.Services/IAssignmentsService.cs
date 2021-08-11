namespace DesignAndBuilding.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Models;

    public interface IAssignmentsService
    {
        Task CreateAssignmentAsync(string description, DateTime endDate, DesignerType designerType, int buildingId);

        Task<Assignment> GetAssignmentById(int id);

        List<Assignment> GetAllAssignmentsForDesignerType(DesignerType designerType);

        List<Assignment> GetAssignmentsWhereUserPlacedBid(string userId);

        ICollection<string> GetAllUsersBidInAssignment(int assignmentId);

        Task RemoveAssignment(int assignmentId);

        bool HasUserCreatedAssignment(string userId, int assignmentId);

        Task EditAssignment(DesignerType designerType, string description, DateTime endDate, int id);
    }
}
