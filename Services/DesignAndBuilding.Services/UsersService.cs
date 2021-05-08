namespace DesignAndBuilding.Services
{
    using System.Linq;

    using DesignAndBuilding.Data;
    using DesignAndBuilding.Data.Models;

    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext db;

        public UsersService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ApplicationUser GetUserById(string id)
        {
            return this.db.Users.FirstOrDefault(x => x.Id == id);
        }
    }
}
