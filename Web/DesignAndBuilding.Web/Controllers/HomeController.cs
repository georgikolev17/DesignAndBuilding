namespace DesignAndBuilding.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Services;
    using DesignAndBuilding.Web.ViewModels;
    using DesignAndBuilding.Web.ViewModels.Assignment;
    using DesignAndBuilding.Web.ViewModels.Building;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAssignmentsService assignmentsService;
        private readonly IUsersService usersService;

        public HomeController(UserManager<ApplicationUser> userManager, IAssignmentsService assignmentsService, IUsersService usersService)
        {
            this.userManager = userManager;
            this.assignmentsService = assignmentsService;
            this.usersService = usersService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user != null && user.DesignerType != DesignerType.Architect)
            {
                var userBids = this.assignmentsService.GetAssignmentsWhereUserPlacedBid(user.Id);
                var assignments = this.assignmentsService
                    .GetAllAssignmentsForDesignerType(user.DesignerType)
                    .Where(x => !x.IsFinished)
                    .Select(x => new BuildingDetailsAssignmentViewModel
                    {
                        BuildingName = x.Building.Name,
                        CreatedOn = x.CreatedOn,
                        ArchitectName = this.usersService.GetUserById(x.Building.ArchitectId).FirstName + " " + this.usersService.GetUserById(x.Building.ArchitectId).LastName,
                        Description = x.Description,
                        DesignerType = x.DesignerType,
                        EndDate = x.EndDate,
                        Id = x.Id,
                        UserPlacedBid = userBids.Contains(x),
                    }).ToList();
                var engineerAssignmentsViewModel = new EngineerAssignmentsViewModel
                {
                    Assignments = assignments,
                    DesignerType = user.DesignerType,
                };

                return this.View("EngineerIndex", engineerAssignmentsViewModel);
            }

            return this.View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> MyBids()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var assignments = this.assignmentsService
                    .GetAssignmentsWhereUserPlacedBid(user.Id)
                    .Select(x => new BuildingDetailsAssignmentViewModel
                    {
                        BuildingName = x.Building.Name,
                        CreatedOn = x.CreatedOn,
                        ArchitectName = this.usersService.GetUserById(x.Building.ArchitectId).FirstName + " " + this.usersService.GetUserById(x.Building.ArchitectId).LastName,
                        Description = x.Description,
                        DesignerType = x.DesignerType,
                        EndDate = x.EndDate,
                        Id = x.Id,
                    }).ToList();
            var engineerAssignmentsViewModel = new EngineerAssignmentsViewModel
            {
                Assignments = assignments,
                DesignerType = user.DesignerType,
            };
            return this.View(engineerAssignmentsViewModel);
        }
    }
}
