namespace DesignAndBuilding.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Models;

    public interface IBidsService
    {
        Task CreateBidAsync(string userId, int assignmentId, decimal bidPrice);
    }
}
