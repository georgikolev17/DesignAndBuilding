namespace DesignAndBuilding.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Services;
    using DesignAndBuilding.Web.ViewModels;
    using DesignAndBuilding.Web.ViewModels.Building;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class BuildingsController : BaseController
    {
        private readonly IBuildingsService buildingsService;
        private readonly IUsersService usersService;
        private readonly UserManager<ApplicationUser> userManager;

        public BuildingsController(IBuildingsService buildingsService, IUsersService usersService, UserManager<ApplicationUser> userManager)
        {
            this.buildingsService = buildingsService;
            this.usersService = usersService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Create()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.DesignerType != DesignerType.Architect)
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Only architects can create new buildings!" });
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BuildingInputModel buildingInputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            if (user.DesignerType != DesignerType.Architect)
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Only architects can create new buildings!" });
            }

            var userId = await this.userManager.GetUserIdAsync(await this.userManager.GetUserAsync(this.User));

            await this.buildingsService.CreateBuildingAsync(userId, buildingInputModel.Name, buildingInputModel.Town, buildingInputModel.TotalBuildUpArea, buildingInputModel.BuildingType);

            return this.Redirect("mybuildings");
        }

        public async Task<IActionResult> MyBuildings()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.DesignerType != DesignerType.Architect)
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Only architects can access this page!" });
            }

            return this.View(this.buildingsService.GetAllBuildingsOfCurrentUserById(user.Id));
        }

        public async Task<IActionResult> Details(int id)
        {
            var building = await this.buildingsService.GetBuildingById(id);
            var buildingViewModel = new BuildingDetailsViewModel()
            {
                Id = id,
                Name = building.Name,
                BuildingType = building.BuildingType,
                TotalBuildUpArea = building.TotalBuildUpArea,
                Assignments = building.Assignments.Select(a => new BuildingDetailsAssignmentViewModel
                {
                    Id = a.Id,
                    CreatedOn = a.CreatedOn,
                    Description = a.Description,
                    DesignerType = a.DesignerType,
                    EndDate = a.EndDate,
                }),
            };
            return this.View(buildingViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!await this.buildingsService.HasUserCreatedBuilding(user.Id, id))
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал заданието, може да го редактира" });
            }

            var building = await this.buildingsService.GetBuildingById(id);

            var buildingViewModel = new BuildingInputModel()
            {
                BuildingType = building.BuildingType.ToString(),
                Name = building.Name,
                TotalBuildUpArea = building.TotalBuildUpArea,
                Town = building.Town,
            };

            return this.View(buildingViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, BuildingInputModel viewModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!await this.buildingsService.HasUserCreatedBuilding(user.Id, id))
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал проекта, може да го редактира" });
            }

            await this.buildingsService.EditBuilding(id, viewModel.BuildingType, viewModel.TotalBuildUpArea, viewModel.Town, viewModel.Name);

            return this.Redirect("/");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!await this.buildingsService.HasUserCreatedBuilding(user.Id, id))
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал проекта, може да го изтрие!" });
            }

            await this.buildingsService.DeleteBuilding(id);

            return this.Redirect("/buildings/mybuildings");
        }
    }
}
