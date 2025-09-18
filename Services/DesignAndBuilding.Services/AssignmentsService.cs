namespace DesignAndBuilding.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Common.Repositories;
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Services.DTOs;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class AssignmentsService : IAssignmentsService
    {
        private readonly IDeletableEntityRepository<Assignment> assignmentsRepository;
        private readonly IDeletableEntityRepository<DescriptionFile> filesRepository;
        private readonly IFilesService filesService;

        public AssignmentsService(IDeletableEntityRepository<Assignment> assignmentsRepository, IDeletableEntityRepository<DescriptionFile> filesRepository, IUsersService usersService, IFilesService filesService)
        {
            this.assignmentsRepository = assignmentsRepository;
            this.filesRepository = filesRepository;
            this.filesService = filesService;
        }

        public AssignmentsService()
        {
        }

        public async Task CreateAssignmentAsync(List<IFormFile> description, DateTime endDate, UserType userType, int buildingId, UserType creatorType)
        {
            var assignment = new Assignment()
            {
                BuildingId = buildingId,
                EndDate = endDate,
                UserType = userType,
                AssignmentType = creatorType == UserType.Architect ? AssignmentType.DesignAsignment : AssignmentType.InvestmentAssignment,
            };

            await this.assignmentsRepository.AddAsync(assignment);
            await this.assignmentsRepository.SaveChangesAsync();

            var descriptionDtos = new List<NewDescriptionFileDto>();
            foreach (var desc in description)
            {
                using var stream = new MemoryStream();
                desc.CopyTo(stream);
                descriptionDtos.Add(new NewDescriptionFileDto(stream.ToArray(), desc.FileName, desc.ContentType, assignment.Id));
            }

            await this.filesService.AddDescriptionFilesAsync(descriptionDtos);
        }

        // WARNING: This functionality does not work now. Decide whether to support it.
        public async Task EditAssignment(UserType UserType, List<IFormFile> description, DateTime endDate, int id)
        {
            //var assignment = await this.assignmentsRepository.All().FirstOrDefaultAsync(x => x.Id == id);

            //// Check if such assignment exists
            //if (assignment == null)
            //{
            //    return;
            //}

            //var files = await this.GetDescriptionFiles(description, assignment);

            //foreach (var file in assignment.Description)
            //{
            //    if (!files.Contains(file))
            //    {
            //        file.IsDeleted = true;
            //        this.filesRepository.All().FirstOrDefault(x => x == file).IsDeleted = true;
            //    }
            //}

            //foreach (var file in files)
            //{
            //    assignment.Description.Add(file);
            //}

            //assignment.UserType = UserType;
            //assignment.EndDate = endDate;

            //await this.filesRepository.SaveChangesAsync();
            //await this.assignmentsRepository.SaveChangesAsync();
        }

        public List<Assignment> GetAllAssignmentsForUserType(UserType UserType, string userId)
        {
            var assignments = this.assignmentsRepository
                .All()
                .Where(x => x.UserType == UserType && x.AssignmentType == AssignmentType.DesignAsignment)
                .Include(x => x.Building)
                .ThenInclude(x => x.Architect)
                .Include(x => x.Bids)
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
                .ThenInclude(x => x.Designer)
                .Include(x => x.Building)
                .Include(x => x.Description)
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
            // Remove associated description files
            var files = await this.GetFilesForAssignment(assignmentId);
            foreach (var file in files)
            {
                // Remove file from object storage
                await this.filesService.DeleteDescriptionFileAsync(file.Name, assignmentId);
            }

            this.assignmentsRepository.All().FirstOrDefault(x => x.Id == assignmentId).IsDeleted = true;
            await this.assignmentsRepository.SaveChangesAsync();
        }

        // TODO: There is the same method in DescriptionFilesService. Refactor.
        public async Task<IEnumerable<DescriptionFile>> GetFilesForAssignment(int assignmentId)
        {
            return await this.filesRepository.All().Where(x => x.AssignmentId == assignmentId).ToListAsync();
        }

        public ICollection<Assignment> GetAllInvestmentAssignments()
        {
            return this.assignmentsRepository.All()
                .Where(x => x.AssignmentType == AssignmentType.InvestmentAssignment)
                .Include(x => x.Building)
                .ToList();
        }
    }
}
