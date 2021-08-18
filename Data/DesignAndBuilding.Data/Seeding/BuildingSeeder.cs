namespace DesignAndBuilding.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Models;

    public class BuildingSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var architectId = dbContext.Users.FirstOrDefault(x => x.Email == "architect@abv.bg").Id;

            var building = new Building()
            {
                ArchitectId = architectId,
                BuildingType = BuildingType.Other,
                Name = "Seeded Building Tower",
                TotalBuildUpArea = 5000,
                Town = "Plovdiv",
            };

            if (dbContext.Buildings.Any(x => x.Name == "Seeded Building Tower"))
            {
                return;
            }

            await dbContext.Buildings.AddAsync(building);

            await dbContext.SaveChangesAsync();
        }
    }
}
