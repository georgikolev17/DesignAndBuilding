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
        private readonly IBuildingsService buildingsService;

        public AssignmentsService(IDeletableEntityRepository<Assignment> assignmentsRepository, IBuildingsService buildingsService)
        {
            this.assignmentsRepository = assignmentsRepository;
            this.buildingsService = buildingsService;
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

        public List<Assignment> GetAllAssignmentsForDesignerType(DesignerType designerType)
        {
            return this.assignmentsRepository
                .All()
                .Where(x => x.DesignerType == designerType)
                .ToList();
        }

        public async Task<Assignment> GetAssignmentById(int id)
        {
            return await this.assignmentsRepository
                .All()
                .Include(x => x.Bids)
                .Include(x => x.Building)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
