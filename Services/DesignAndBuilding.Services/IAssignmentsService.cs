namespace DesignAndBuilding.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.ViewModels.Assignment;
    using DesignAndBuilding.Web.ViewModels.Building;
    using Microsoft.AspNetCore.Http;

    public interface IAssignmentsService
    {
        ICollection<Assignment> GetAllInvestmentAssignments();

        Task<Assignment> CreateAssignmentAsync(AssignmentInputModel inputAssignment);

        Task<Assignment> GetAssignmentById(int id);

        List<Assignment> GetAllAssignmentsForUserType(UserType userType);

        List<Assignment> GetAssignmentsWhereUserPlacedBid(string userId);

        ICollection<string> GetAllUsersBidInAssignment(int assignmentId);

        Task RemoveAssignment(int assignmentId);

        bool HasUserCreatedAssignment(string userId, int assignmentId);

        Task EditAssignment(UserType UserType, List<IFormFile> description, DateTime endDate, int id);

        int GetAssignmentsCount();

        Task<IEnumerable<DescriptionFile>> GetFilesForAssignment(int assignmentId);
    }
}
