namespace DesignAndBuilding.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using DesignAndBuilding.Common;
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Services;
    using DesignAndBuilding.Web.ViewModels;
    using DesignAndBuilding.Web.ViewModels.Assignment;
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
        private readonly IMapper mapper;

        public BuildingsController(IBuildingsService buildingsService, IUsersService usersService, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.buildingsService = buildingsService;
            this.usersService = usersService;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Create()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            // Check if current user is architect or investor
            if (user.UserType != UserType.Architect)
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само архитекти и инвеститори могат да създават обекти!" });
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BuildingInputModel buildingInputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            // Validate input data
            if (!this.ModelState.IsValid)
            {
                return this.View(buildingInputModel);
            }

            // Check if current user is architect or investor
            if (user.UserType != UserType.Architect)
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само архитекти и инвеститори могат да създават обекти!" });
            }

            var userId = await this.userManager.GetUserIdAsync(await this.userManager.GetUserAsync(this.User));

            await this.buildingsService.CreateBuildingAsync(userId, buildingInputModel.Name, buildingInputModel.Town, buildingInputModel.TotalBuildUpArea, buildingInputModel.BuildingType);

            return this.Redirect("mybuildings");
        }

        public async Task<IActionResult> MyBuildings()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            // Check if current user is architect
            if (user.UserType != UserType.Architect)
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само архитекти и инвеститори могат да достъпват тази страница!" });
            }

            return this.View(this.buildingsService.GetAllBuildingsOfCurrentUserById(user.Id));
        }

        public async Task<IActionResult> Details(int id)
        {
            var building = await this.buildingsService.GetBuildingById(id);

            var user = await this.userManager.GetUserAsync(this.User);

            if (building == null)
            {
                return this.NotFound();
            }

            // Check if current user is author of the building
            if (building.ArchitectId != user.Id && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал проекта, може да вижда детейлите му!" });
            }

            var buildingViewModel = this.mapper.Map<BuildingDetailsViewModel>(building);

            return this.View(buildingViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var building = await this.buildingsService.GetBuildingById(id);

            if (building == null)
            {
                return this.NotFound(0);
            }

            // Check if current user is author of the assignment
            if (!await this.buildingsService.HasUserCreatedBuilding(user.Id, id) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал проекта, може да го редактира!" });
            }

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

            if (await this.buildingsService.GetBuildingById(id) == null)
            {
                return this.NotFound();
            }

            // Check if current user is author of the assignment
            if (!await this.buildingsService.HasUserCreatedBuilding(user.Id, id) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал проекта, може да го редактира!" });
            }

            await this.buildingsService.EditBuilding(id, viewModel.BuildingType, viewModel.TotalBuildUpArea, viewModel.Town, viewModel.Name);

            return this.Redirect("/buildings/mybuildings");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (await this.buildingsService.GetBuildingById(id) == null)
            {
                return this.NotFound();
            }

            // Check if current user is author of the assignment
            if (!await this.buildingsService.HasUserCreatedBuilding(user.Id, id) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал проекта, може да го изтрие!" });
            }

            await this.buildingsService.DeleteBuilding(id);

            return this.Redirect("/buildings/mybuildings");
        }
    }
}
