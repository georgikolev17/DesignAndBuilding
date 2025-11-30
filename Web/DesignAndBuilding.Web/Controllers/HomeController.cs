namespace DesignAndBuilding.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Common;
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Services;
    using DesignAndBuilding.Services.Messaging;
    using DesignAndBuilding.Web.ViewModels;
    using DesignAndBuilding.Web.ViewModels.Assignment;
    using DesignAndBuilding.Web.ViewModels.Building;
    using DesignAndBuilding.Web.ViewModels.Notification;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    public class HomeController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAssignmentsService assignmentsService;
        private readonly IUsersService usersService;
        private readonly INotificationsService notificationsService;

        public HomeController(UserManager<ApplicationUser> userManager, IAssignmentsService assignmentsService, IUsersService usersService, INotificationsService notificationsService, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.assignmentsService = assignmentsService;
            this.usersService = usersService;
            this.notificationsService = notificationsService;
            //Task.Run(async () =>
            //{
            //    try
            //    {
            //        await emailSender.SendEmailAsync(
            //            to: "g.t.kolev1@gmail.com",        // test target
            //            subject: "Test email from BuildNet",
            //            htmlContent: "<h2>Hello!</h2><p>This is a test email from your ASP.NET Core app.</p>"
            //        );
            //        Console.WriteLine("Test email sent successfully.");
            //    }
            //    catch (System.Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //});
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user != null && user.UserType != UserType.Architect && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                List<BuildingDetailsAssignmentViewModel> assignments = new List<BuildingDetailsAssignmentViewModel>();
                assignments = this.assignmentsService
                    .GetAllAssignmentsForUserType(user.UserType, user.Id)
                    .Select(x => new BuildingDetailsAssignmentViewModel
                    {
                        BuildingName = x.Building.Name,
                        CreatedOn = x.CreatedOn,
                        ArchitectName = this.usersService.GetUserById(x.Building.ArchitectId).FullNameWithTitle,
                        Description = x.Description,
                        UserType = x.UserType,
                        EndDate = x.EndDate,
                        Id = x.Id,
                        UserPlacedBid = this.assignmentsService.GetAssignmentsWhereUserPlacedBid(user.Id).Contains(x),
                        BestBid = x.Bids.OrderBy(x => x.Price).FirstOrDefault() == null ? null : x.Bids.OrderBy(x => x.Price).FirstOrDefault().Price,
                        UserBestBid = x.Bids.Where(x => x.DesignerId == user.Id).OrderBy(x => x.Price).FirstOrDefault() != null ? x.Bids.Where(x => x.DesignerId == user.Id).OrderBy(x => x.Price).FirstOrDefault().Price : null,
                    }).ToList();

                var assignmentsViewModel = new EngineerAssignmentsViewModel
                {
                    Assignments = assignments,
                    UserType = user.UserType,
                };

                return this.View("EngineerIndex", assignmentsViewModel);
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
                        ArchitectName = this.usersService.GetUserById(x.Building.ArchitectId).FullNameWithTitle,
                        Description = x.Description,
                        UserType = x.UserType,
                        EndDate = x.EndDate,
                        Id = x.Id,
                        BestBid = x.Bids.OrderBy(x => x.Price).FirstOrDefault().Price,
                        UserBestBid = x.Bids.Where(x => x.DesignerId == user.Id).OrderBy(x => x.Price).FirstOrDefault().Price,
                        UserPlacedBid = true,
                    }).ToList();

            var engineerAssignmentsViewModel = new EngineerAssignmentsViewModel
            {
                Assignments = assignments,
                UserType = user.UserType,
            };
            return this.View(engineerAssignmentsViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Notifications()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var viewModel = this.notificationsService.GetNotificationsForUser(user.Id);

            return this.View(new AllNotificationsViewModel() { Notifications = viewModel, UserId = user.Id });
        }
    }
}
