namespace DesignAndBuilding.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data;
    using DesignAndBuilding.Data.Common.Repositories;
    using DesignAndBuilding.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task<string?> GetEmailByIdAsync(string id)
        {
            return (await this.usersRepository.All()
                .FirstOrDefaultAsync(x => x.Id == id))?.Email;
        }

        public async Task<IEnumerable<string>> GetEmailsOfUsersOfTypeAsync(UserType userType)
        {
            return await this.usersRepository.All()
                .Where(x => x.UserType == userType)
                .Select(x => x.Email)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetIdsOfUsersOfTypeAsync(UserType userType)
        {
            return await this.usersRepository.All()
                .Where(x => x.UserType == userType)
                .Select(x => x.Id)
                .ToListAsync();
        }

        public ApplicationUser GetUserById(string id)
        {
            return this.usersRepository.All().FirstOrDefault(x => x.Id == id);
        }

        public int GetUsersCount()
        {
            return this.usersRepository.All().Count();
        }
    }
}
