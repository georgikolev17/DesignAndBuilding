namespace DesignAndBuilding.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Services;
    using DesignAndBuilding.Web.ViewModels;
    using DesignAndBuilding.Web.ViewModels.Assignment;
    using DesignAndBuilding.Web.ViewModels.Bid;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class AssignmentsController : Controller
    {
        private readonly IAssignmentsService assignmentsService;
        private readonly IBidsService bidsService;
        private readonly INotificationsService notificationsService;
        private readonly UserManager<ApplicationUser> userManager;

        public AssignmentsController(IAssignmentsService assignmentsService, IBidsService bidsService, INotificationsService notificationsService, UserManager<ApplicationUser> userManager)
        {
            this.assignmentsService = assignmentsService;
            this.bidsService = bidsService;
            this.notificationsService = notificationsService;
            this.userManager = userManager;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AssignmentInputModel assignment)
        {
            if (!this.ModelState.IsValid || assignment.EndDate < DateTime.UtcNow)
            {
                return this.View();
            }

            await this.assignmentsService.CreateAssignmentAsync(assignment.Description, assignment.EndDate, assignment.DesignerType, assignment.BuildingId);
            return this.RedirectToAction("Details", "Buildings", new { id = assignment.BuildingId });
        }

        public async Task<IActionResult> Details(int id)
        {
            var assignment = await this.assignmentsService.GetAssignmentById(id);

            return this.View(new AssignmentViewModel()
            {
                CreatedOn = assignment.CreatedOn,
                Description = assignment.Description,
                DesignerType = assignment.DesignerType,
                EndDate = assignment.EndDate,
                Building = new AssignmentBuildingViewModel()
                {
                    TotalBuildUpArea = assignment.Building.TotalBuildUpArea,
                    BuildingType = assignment.Building.BuildingType,
                    Name = assignment.Building.Name,
                },
                Bids = assignment.Bids
                    .Select(x => new AssignmentBidViewModel()
                    {
                        Price = x.Price,
                        TimePlaced = x.TimePlaced,
                    })
                    .OrderBy(x => x.Price)
                    .ToList(),
            });
        }

        // Place bids
        [HttpPost]
        public async Task<IActionResult> Details(PlaceBidViewModel bidViewModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var assignment = await this.assignmentsService.GetAssignmentById(bidViewModel.Id);

            if (user.DesignerType != assignment.DesignerType)
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = $"Only {assignment.DesignerType}s can make bids for this assignment!" });
            }

            if (!this.ModelState.IsValid)
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = $"Invalid bid" });
            }

            await this.bidsService.CreateBidAsync(user.Id, bidViewModel.Id, decimal.Parse(bidViewModel.BidPrice));

            var usersToSendNotification = this.assignmentsService.GetAllUsersBidInAssignment(assignment.Id);

            // Assignment creator should also recieve notification
            usersToSendNotification.Add(assignment.Building.ArchitectId);

            // User who placed bid will recieve another notification
            usersToSendNotification.Remove(user.Id);
            var userPlacedBid = new List<string>() { user.Id };
            await this.notificationsService.AddNotificationAsync(userPlacedBid, $"Вие наддадохте с {bidViewModel.BidPrice} лв. за {assignment.Building.Name}!");

            // All other users placed bids for this assignment will recieve this notification
            await this.notificationsService.AddNotificationAsync(usersToSendNotification, $"Има ново наддаване в {assignment.Building.Name} - {bidViewModel.BidPrice} лв.");

            return this.RedirectToAction("Details", "Assignments");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!this.assignmentsService.HasUserCreatedAssignment(user.Id, id))
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал заданието, може да го редактира" });
            }

            var assignment = await this.assignmentsService.GetAssignmentById(id);

            var assignmentViewModel = new AssignmentInputModel()
            {
                BuildingId = assignment.BuildingId,
                Description = assignment.Description,
                DesignerType = assignment.DesignerType,
                EndDate = assignment.EndDate,
            };

            return this.View(assignmentViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AssignmentInputModel viewModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!this.assignmentsService.HasUserCreatedAssignment(user.Id, id))
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал заданието, може да го редактира" });
            }

            await this.assignmentsService.EditAssignment(viewModel.DesignerType, viewModel.Description, viewModel.EndDate, id);

            return this.Redirect("/");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!this.assignmentsService.HasUserCreatedAssignment(user.Id, id))
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал заданието, може да го изтрие!" });
            }

            await this.assignmentsService.RemoveAssignment(id);

            return this.Redirect("/");
        }
    }
}
