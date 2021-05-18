using DesignAndBuilding.Data.Common.Repositories;
using DesignAndBuilding.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndBuilding.Services
{
    public class AssignmentsService : IAssignmentsService
    {
        private readonly IDeletableEntityRepository<Assignment> assignmentsRepository;

        public AssignmentsService(IDeletableEntityRepository<Assignment> assignmentsRepository)
        {
            this.assignmentsRepository = assignmentsRepository;
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
    }
}
