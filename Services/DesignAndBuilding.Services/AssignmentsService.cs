namespace DesignAndBuilding.Services
{
    using System;
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

        public async Task CreateAssignmentAsync(string description, DateTime endDate, DesignerType designerType, decimal basePricePerSquareMeter, int buildingId)
        {
            var assignment = new Assignment()
            {
                BasePricePerSquareMeter = basePricePerSquareMeter,
                BuildingId = buildingId,
                Description = description,
                EndDate = endDate,
                DesignerType = designerType,
            };

            await this.assignmentsRepository.AddAsync(assignment);
            await this.assignmentsRepository.SaveChangesAsync();
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
