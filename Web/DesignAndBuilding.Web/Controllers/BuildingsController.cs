namespace DesignAndBuilding.Web.Controllers
{
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Services;
    using DesignAndBuilding.Web.ViewModels;
    using DesignAndBuilding.Web.ViewModels.Building;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

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

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null || user.DesignerType != DesignerType.Architect)
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Only architects can create new buildings!" });
            }

            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(BuildingInputModel buildingInputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            if (user == null || user.DesignerType != DesignerType.Architect)
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Only architects can create new buildings!" });
            }

            var userId = await this.userManager.GetUserIdAsync(await this.userManager.GetUserAsync(this.User));

            await this.buildingsService.CreateBuildingAsync(userId, buildingInputModel.Name, buildingInputModel.Town, buildingInputModel.TotalBuildUpArea, buildingInputModel.BuildingType);

            return this.Redirect("mybuildings");
        }

        [Authorize]
        public async Task<IActionResult> MyBuildings()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null || user.DesignerType != DesignerType.Architect)
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Only architects can access this page!" });
            }

            return this.View(this.buildingsService.GetAllBuildingsOfCurrentUserById(user.Id));
        }
    }
}
