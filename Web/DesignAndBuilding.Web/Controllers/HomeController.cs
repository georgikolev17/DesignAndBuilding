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
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAssignmentsService assignmentsService;

        public HomeController(UserManager<ApplicationUser> userManager, IAssignmentsService assignmentsService)
        {
            this.userManager = userManager;
            this.assignmentsService = assignmentsService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user != null && user.DesignerType != DesignerType.Architect)
            {
                var assignments = this.assignmentsService
                    .GetAllAssignmentsForDesignerType(user.DesignerType)
                    .Select(x => new BuildingDetailsAssignmentViewModel
                    {
                        CreatedOn = x.CreatedOn,
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

                return this.View("EngineerIndex", engineerAssignmentsViewModel);
            }

            return this.View();
        }

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
    }
}
