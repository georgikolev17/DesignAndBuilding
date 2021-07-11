namespace DesignAndBuilding.Web.Controllers
{
    using System;
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
        private readonly UserManager<ApplicationUser> userManager;

        public AssignmentsController(IAssignmentsService assignmentsService, IBidsService bidsService, UserManager<ApplicationUser> userManager)
        {
            this.assignmentsService = assignmentsService;
            this.bidsService = bidsService;
            this.userManager = userManager;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AssignmentInputModel assignment)
        {
            if (!this.ModelState.IsValid || assignment.EndDate > DateTime.UtcNow)
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
            return this.RedirectToAction("Details", "Assignments");
        }
    }
}
