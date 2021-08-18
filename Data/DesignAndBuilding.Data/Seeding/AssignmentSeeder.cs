namespace DesignAndBuilding.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Models;

    public class AssignmentSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var buildingId = dbContext.Buildings.FirstOrDefault(x => x.Name == "Seeded Building Tower").Id;

            var assignment = new Assignment()
            {
                BuildingId = buildingId,
                Description = "That is seeded assignment for Electro Engineers",
                DesignerType = DesignerType.ElectroEngineer,
                EndDate = DateTime.Now + TimeSpan.FromDays(7),
            };

            if (dbContext.Assignments.Any(x => x.Description == "That is seeded assignment for Electro Engineers"))
            {
                return;
            }

            await dbContext.Assignments.AddAsync(assignment);

            await dbContext.SaveChangesAsync();
        }
    }
}
