namespace DesignAndBuilding.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Common.Repositories;
    using DesignAndBuilding.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class AssignmentsService : IAssignmentsService
    {
        private readonly IDeletableEntityRepository<Assignment> assignmentsRepository;

        public AssignmentsService(IDeletableEntityRepository<Assignment> assignmentsRepository)
        {
            this.assignmentsRepository = assignmentsRepository;
        }

        public async Task CreateAssignmentAsync(string description, DateTime endDate, DesignerType designerType, int buildingId)
        {
            var assignment = new Assignment()
            {
                BuildingId = buildingId,
                Description = description,
                EndDate = endDate,
                DesignerType = designerType,
            };

            await this.assignmentsRepository.AddAsync(assignment);
            await this.assignmentsRepository.SaveChangesAsync();
        }

        public async Task EditAssignment(DesignerType designerType, string description, DateTime endDate, int id)
        {
            var assignment = await this.assignmentsRepository.All().FirstOrDefaultAsync(x => x.Id == id);

            if (assignment == null)
            {
                return;
            }

            assignment.Description = description;
            assignment.DesignerType = designerType;
            assignment.EndDate = endDate;

            await this.assignmentsRepository.SaveChangesAsync();
        }

        public List<Assignment> GetAllAssignmentsForDesignerType(DesignerType designerType)
        {
            var assignments = this.assignmentsRepository
                .All()
                .Include(x => x.Building)
                .Where(x => x.DesignerType == designerType && x.EndDate >= DateTime.Now)
                .OrderBy(x => x.EndDate)
                .ToList();

            return assignments;
        }

        public ICollection<string> GetAllUsersBidInAssignment(int assignmentId)
        {
            return this.assignmentsRepository
                .All()
                .FirstOrDefault(x => x.Id == assignmentId)
                .Bids
                .Select(x => x.DesignerId)
                .Distinct()
                .ToList();
        }

        public async Task<Assignment> GetAssignmentById(int id)
        {
            var assignment = await this.assignmentsRepository
                .All()
                .Include(x => x.Bids)
                .Include(x => x.Building)
                .FirstOrDefaultAsync(x => x.Id == id);

            return assignment;
        }

        public List<Assignment> GetAssignmentsWhereUserPlacedBid(string userId)
        {
            var assignments = this.
                assignmentsRepository.All()
                .Where(x => x.Bids.Any(b => b.DesignerId == userId))
                .Include(x => x.Building)
                .ToList();

            return assignments;
        }

        public bool HasUserCreatedAssignment(string userId, int assignmentId)
        {
            return this.assignmentsRepository.All().Any(a => a.Building.ArchitectId == userId && a.Id == assignmentId);
        }

        public async Task RemoveAssignment(int assignmentId)
        {
            this.assignmentsRepository.All().FirstOrDefault(x => x.Id == assignmentId).IsDeleted = true;
            await this.assignmentsRepository.SaveChangesAsync();
        }
    }
}
