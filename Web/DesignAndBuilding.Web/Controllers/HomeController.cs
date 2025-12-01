namespace DesignAndBuilding.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using DesignAndBuilding.Common;
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Services;
    using DesignAndBuilding.Services.Messaging;
    using DesignAndBuilding.Web.ViewModels;
    using DesignAndBuilding.Web.ViewModels.Assignment;
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
        private readonly IMapper mapper;

        public HomeController(UserManager<ApplicationUser> userManager, IAssignmentsService assignmentsService, IUsersService usersService, INotificationsService notificationsService, IMapper mapper)
        {
            this.userManager = userManager;
            this.assignmentsService = assignmentsService;
            this.usersService = usersService;
            this.notificationsService = notificationsService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user != null && user.UserType != UserType.Architect && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                var assignmentsList = this.assignmentsService.GetAllAssignmentsForUserType(user.UserType);

                var assignments = this.mapper.Map<List<AssignmentListViewModel>>(
                    assignmentsList,
                    opt => opt.Items["UserId"] = user.Id
                );

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

            var assignmentsList = this.assignmentsService.GetAllAssignmentsForUserType(user.UserType);

            var assignments = this.mapper.Map<List<AssignmentListViewModel>>(
                assignmentsList,
                opt => opt.Items["UserId"] = user.Id
            );

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
