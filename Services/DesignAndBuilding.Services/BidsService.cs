namespace DesignAndBuilding.Services
{
    using System;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Common.Repositories;
    using DesignAndBuilding.Data.Models;

    public class BidsService : IBidsService
    {
        private readonly IDeletableEntityRepository<Bid> bidsRepository;

        public BidsService(IDeletableEntityRepository<Bid> bidsRepository)
        {
            this.bidsRepository = bidsRepository;
        }

        public async Task CreateBidAsync(string userId, int assignmentId, decimal bidPrice)
        {
            await this.bidsRepository.AddAsync(new Bid
            {
                AssignmentId = assignmentId,
                DesignerId = userId,
                TimePlaced = DateTime.Now,
                Price = bidPrice,
            });

            await this.bidsRepository.SaveChangesAsync();
        }
    }
}
