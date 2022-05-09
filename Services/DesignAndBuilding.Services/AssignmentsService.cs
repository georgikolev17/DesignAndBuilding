namespace DesignAndBuilding.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Common.Repositories;
    using DesignAndBuilding.Data.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class AssignmentsService : IAssignmentsService
    {
        private readonly IDeletableEntityRepository<Assignment> assignmentsRepository;
        private readonly IDeletableEntityRepository<DescriptionFile> filesRepository;

        public AssignmentsService(IDeletableEntityRepository<Assignment> assignmentsRepository, IDeletableEntityRepository<DescriptionFile> filesRepository, IUsersService usersService)
        {
            this.assignmentsRepository = assignmentsRepository;
            this.filesRepository = filesRepository;
        }

        public AssignmentsService()
        {
        }

        public async Task CreateAssignmentAsync(List<IFormFile> description, DateTime endDate, UserType UserType, int buildingId)
        {
            var assignment = new Assignment()
            {
                BuildingId = buildingId,
                EndDate = endDate,
                UserType = UserType,
            };

            assignment.Description = await this.GetDescriptionFiles(description, assignment);

            await this.assignmentsRepository.AddAsync(assignment);
            await this.assignmentsRepository.SaveChangesAsync();
        }

        public async Task EditAssignment(UserType UserType, List<IFormFile> description, DateTime endDate, int id)
        {
            var assignment = await this.assignmentsRepository.All().FirstOrDefaultAsync(x => x.Id == id);

            // Check if such assignment exists
            if (assignment == null)
            {
                return;
            }

            var files = await this.GetDescriptionFiles(description, assignment);

            foreach (var file in assignment.Description)
            {
                if (!files.Contains(file))
                {
                    file.IsDeleted = true;
                    this.filesRepository.All().FirstOrDefault(x => x == file).IsDeleted = true;
                }
            }

            foreach (var file in files)
            {
                assignment.Description.Add(file);
            }

            assignment.UserType = UserType;
            assignment.EndDate = endDate;

            await this.filesRepository.SaveChangesAsync();
            await this.assignmentsRepository.SaveChangesAsync();
        }

        public List<Assignment> GetAllAssignmentsForUserType(UserType UserType, string userId)
        {
            var assignments = this.assignmentsRepository
                .All()
                .Where(x => x.UserType == UserType)
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
            this.assignmentsRepository.All().FirstOrDefault(x => x.Id == assignmentId).IsDeleted = true;
            await this.assignmentsRepository.SaveChangesAsync();
        }

        public async Task<ICollection<DescriptionFile>> GetDescriptionFiles(List<IFormFile> files, Assignment assignment)
        {
            var descriptionFiles = new List<DescriptionFile>();

            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    // IFormFile to DescriptionFile
                    using var stream = new MemoryStream();
                    file.CopyTo(stream);
                    byte[] bytes = stream.ToArray();

                    descriptionFiles.Add(new DescriptionFile()
                    {
                        AssignmentId = assignment.Id,
                        Content = bytes,
                        Name = file.FileName,
                        ContentType = file.ContentType,
                    });
                    stream.Close();
                }
            }

            return descriptionFiles;
        }

        public IEnumerable<DescriptionFile> GetFilesForAssignment(int assignmentId)
        {
            return this.filesRepository.All().Where(x => x.AssignmentId == assignmentId);
        }
    }
}
