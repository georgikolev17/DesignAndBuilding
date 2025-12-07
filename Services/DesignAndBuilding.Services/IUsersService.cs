namespace DesignAndBuilding.Services
{
    using DesignAndBuilding.Data.Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUsersService
    {
        ApplicationUser GetUserById(string id);

        Task<IEnumerable<string>> GetIdsOfUsersOfTypeAsync(UserType userType);

        Task<IEnumerable<string>> GetEmailsOfUsersOfTypeAsync(UserType userType);

        Task<string?> GetEmailByIdAsync(string id);

        int GetUsersCount();
    }
}
