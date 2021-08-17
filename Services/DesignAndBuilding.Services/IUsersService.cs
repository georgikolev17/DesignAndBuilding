namespace DesignAndBuilding.Services
{
    using DesignAndBuilding.Data.Models;

    public interface IUsersService
    {
        ApplicationUser GetUserById(string id);

        int GetUsersCount();
    }
}
