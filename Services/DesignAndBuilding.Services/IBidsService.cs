namespace DesignAndBuilding.Services
{
    using System.Threading.Tasks;

    public interface IBidsService
    {
        Task CreateBidAsync(string userId, int assignmentId, decimal bidPrice);
    }
}
