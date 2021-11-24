namespace DesignAndBuilding.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Common.Repositories;
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.ViewModels.Assignment;
    using DesignAndBuilding.Web.ViewModels.Building;
    using Microsoft.EntityFrameworkCore;

    public class AssignmentsService : IAssignmentsService
    {
        private readonly IDeletableEntityRepository<Assignment> assignmentsRepository;
        private readonly IUsersService usersService;

        public AssignmentsService(IDeletableEntityRepository<Assignment> assignmentsRepository, IUsersService usersService)
        {
            this.assignmentsRepository = assignmentsRepository;
            this.usersService = usersService;
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

        public List<BuildingDetailsAssignmentViewModel> GetAllAssignmentsForDesignerType(DesignerType designerType, string userId, AssignmentSearchInputModel search)
        {
            var assignments = this.assignmentsRepository
                .All()
                .Include(x => x.Building)
                .ThenInclude(x => x.Architect)
                .Include(x => x.Bids)
                .Where(x => x.DesignerType == designerType && (string.IsNullOrEmpty(search.SearchText) || (x.Building.Architect.FirstName + " " + x.Building.Architect.LastName + " " + x.Building.Name + " " + x.Description + " " + x.Building.Town).ToLower().Contains(search.SearchText)) && (search.BuildingType == null || x.Building.BuildingType == search.BuildingType))
                .OrderBy(x => x.EndDate)
                .Select(x => new BuildingDetailsAssignmentViewModel
                {
                    BuildingName = x.Building.Name,
                    CreatedOn = x.CreatedOn,
                    ArchitectName = this.usersService.GetUserById(userId).FirstName + " " + this.usersService.GetUserById(x.Building.ArchitectId).LastName,
                    Description = x.Description,
                    DesignerType = x.DesignerType,
                    EndDate = x.EndDate,
                    Id = x.Id,
                    UserPlacedBid = this.GetAssignmentsWhereUserPlacedBid(userId).Contains(x),
                    BestBid = x.Bids.OrderBy(x => x.Price).FirstOrDefault() == null ? null : x.Bids.OrderBy(x => x.Price).FirstOrDefault().Price,
                    UserBestBid = x.Bids.Where(x => x.DesignerId == userId).OrderBy(x => x.Price).FirstOrDefault() != null ? x.Bids.Where(x => x.DesignerId == userId).OrderBy(x => x.Price).FirstOrDefault().Price : null,
                })
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
                .ThenInclude(x => x.Designer)
                .Include(x => x.Building)
                .FirstOrDefaultAsync(x => x.Id == id);

            return assignment;
        }

        public int GetAssignmentsCount()
        {
            return this.assignmentsRepository.All().Count();
        }

        public List<Assignment> GetAssignmentsWhereUserPlacedBid(string userId)
        {
            var assignments = this.
                assignmentsRepository.All()
                .Where(x => x.Bids.Any(b => b.DesignerId == userId))
                .Include(x => x.Building)
                .Include(x => x.Bids)
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
