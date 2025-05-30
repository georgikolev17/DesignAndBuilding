﻿namespace DesignAndBuilding.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Common;
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

            if (user != null && user.UserType != UserType.Architect && user.UserType != UserType.InvestmentPerson && user.UserType != UserType.InvestmentCompany && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                List<BuildingDetailsAssignmentViewModel> assignments = new List<BuildingDetailsAssignmentViewModel>();

                if (user.UserType == UserType.BuildingCompany)
                {
                    assignments = this.assignmentsService
                       .GetAllInvestmentAssignments()
                       .Select(x => new BuildingDetailsAssignmentViewModel
                       {
                           BuildingType = x.Building.BuildingType,
                           CreatedOn = x.CreatedOn,
                           ArchitectName = this.usersService.GetUserById(x.Building.ArchitectId).CompanyName,
                           Description = x.Description,
                           UserType = x.UserType,
                           EndDate = x.EndDate,
                           Id = x.Id,
                           UserPlacedBid = this.assignmentsService.GetAssignmentsWhereUserPlacedBid(user.Id).Contains(x),
                           BestBid = x.Bids.OrderBy(x => x.Price).FirstOrDefault() == null ? null : x.Bids.OrderBy(x => x.Price).FirstOrDefault().Price,
                           UserBestBid = x.Bids.Where(x => x.DesignerId == user.Id).OrderBy(x => x.Price).FirstOrDefault() != null ? x.Bids.Where(x => x.DesignerId == user.Id).OrderBy(x => x.Price).FirstOrDefault().Price : null,
                       }).ToList();
                }
                else
                {
                    assignments = this.assignmentsService
                        .GetAllAssignmentsForUserType(user.UserType, user.Id)
                        .Select(x => new BuildingDetailsAssignmentViewModel
                        {
                            BuildingType = x.Building.BuildingType,
                            CreatedOn = x.CreatedOn,
                            ArchitectName = this.usersService.GetUserById(x.Building.ArchitectId).FirstName + " " + this.usersService.GetUserById(x.Building.ArchitectId).LastName,
                            Description = x.Description,
                            UserType = x.UserType,
                            EndDate = x.EndDate,
                            Id = x.Id,
                            UserPlacedBid = this.assignmentsService.GetAssignmentsWhereUserPlacedBid(user.Id).Contains(x),
                            BestBid = x.Bids.OrderBy(x => x.Price).FirstOrDefault() == null ? null : x.Bids.OrderBy(x => x.Price).FirstOrDefault().Price,
                            UserBestBid = x.Bids.Where(x => x.DesignerId == user.Id).OrderBy(x => x.Price).FirstOrDefault() != null ? x.Bids.Where(x => x.DesignerId == user.Id).OrderBy(x => x.Price).FirstOrDefault().Price : null,
                        }).ToList();
                }

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
                        BuildingType = x.Building.BuildingType,
                        CreatedOn = x.CreatedOn,
                        ArchitectName = this.usersService.GetUserById(x.Building.ArchitectId).FirstName + " " + this.usersService.GetUserById(x.Building.ArchitectId).LastName,
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
