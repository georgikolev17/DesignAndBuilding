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
    using DesignAndBuilding.Web.ViewModels.Notification;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAssignmentsService assignmentsService;
        private readonly IUsersService usersService;
        private readonly INotificationsService notificationsService;

        public HomeController(UserManager<ApplicationUser> userManager, IAssignmentsService assignmentsService, IUsersService usersService, INotificationsService notificationsService)
        {
            this.userManager = userManager;
            this.assignmentsService = assignmentsService;
            this.usersService = usersService;
            this.notificationsService = notificationsService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user != null && user.DesignerType != DesignerType.Architect)
            {
                var userBids = this.assignmentsService.GetAssignmentsWhereUserPlacedBid(user.Id);
                var activeAssignments = this.assignmentsService
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

                var finishedAssignments = this.assignmentsService
                    .GetAllAssignmentsForDesignerType(user.DesignerType)
                    .Where(x => x.IsFinished)
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
                    ActiveAssignments = activeAssignments,
                    FinishedAssignments = finishedAssignments,
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
                ActiveAssignments = assignments,
                DesignerType = user.DesignerType,
            };
            return this.View(engineerAssignmentsViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Notifications()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var viewModel = this.notificationsService.GetNotificationsForUser(user.Id)
                .Select(n => new NotificationViewModel()
                {
                    Id = n.Id,
                    Date = n.CreatedOn,
                    Message = n.Message,
                    IsRead = n.IsRead,
                });

            return this.View(new AllNotificationsViewModel() { Notifications = viewModel, UserId = user.Id });
        }
    }
}
