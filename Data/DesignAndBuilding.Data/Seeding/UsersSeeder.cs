namespace DesignAndBuilding.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Common;
    using DesignAndBuilding.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            var role = await roleManager.FindByNameAsync(GlobalConstants.UserRoleName);

            await Seed5ElectroEngineers(dbContext, userManager, role);

            await Seed1Architect(dbContext, userManager, role);
        }

        private static async Task Seed5ElectroEngineers(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, ApplicationRole role)
        {
            var firstNames = new string[] { "Ivan", "Georgi", "Petar", "Dimitar", "Kaloyan" };

            var lastNames = new string[] { "Kaloyanov", "Dimitrov", "Georgiev", "Ivanov", "Petrov" };

            var email = string.Empty;

            const string phoneNumber = "0899123456";

            const string password = "123456";

            for (int i = 0; i < 5; i++)
            {
                email = $"electro{i}@abv.bg";

                if (dbContext.Users.Any(x => x.Email == email))
                {
                    continue;
                }

                var user = new ApplicationUser()
                {
                    FirstName = firstNames[i],
                    LastName = lastNames[i],
                    Email = email,
                    UserName = email,
                    UserType = UserType.ElectroEngineer,
                    PhoneNumber = phoneNumber,
                    Password = password,
                    RegistrationNumber = $"EE-{i + 1000}",
                };

                if (dbContext.Users.Any(x => x.Email == email))
                {
                    continue;
                }

                await userManager.CreateAsync(user, password);

                await userManager.AddToRoleAsync(user, role.Name);
            }
        }

        private static async Task Seed1Architect(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, ApplicationRole role)
        {
            var user = new ApplicationUser()
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                Email = "architect@abv.bg",
                UserName = "architect@abv.bg",
                UserType = UserType.Architect,
                PhoneNumber = "0899111111",
                Password = "123456",
                RegistrationNumber = "AR-1000",
            };

            if (dbContext.Users.Any(x => x.Email == "architect@abv.bg"))
            {
                return;
            }

            await userManager.CreateAsync(user, "123456");

            await userManager.AddToRoleAsync(user, role.Name);
        }
    }
}
