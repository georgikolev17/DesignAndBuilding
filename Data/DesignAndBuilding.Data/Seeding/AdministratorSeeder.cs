namespace DesignAndBuilding.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Common;
    using DesignAndBuilding.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class AdministratorSeeder : ISeeder
    {
        private const string AdminEmail = "admin@dab.com";
        private const string AdminPassword = "123456";

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            var role = await roleManager.FindByNameAsync(GlobalConstants.AdministratorRoleName);

            if (role == null)
            {
                await new RolesSeeder().SeedAsync(dbContext, serviceProvider);
            }

            if (dbContext.Users.Any(x => x.Email == AdminEmail))
            {
                return;
            }

            var user = new ApplicationUser()
            {
                Email = AdminEmail,
                UserName = AdminEmail,
                UserType = UserType.Other,
                FirstName = "Admin",
                LastName = "Adminov",
                Password = "123456",
            };

            await userManager.CreateAsync(user, AdminPassword);

            await userManager.AddToRoleAsync(user, role.Name);
        }
    }
}
