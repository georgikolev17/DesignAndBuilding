namespace DesignAndBuilding.Services
{
    using System.Linq;

    using DesignAndBuilding.Data;
    using DesignAndBuilding.Data.Common.Repositories;
    using DesignAndBuilding.Data.Models;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
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
