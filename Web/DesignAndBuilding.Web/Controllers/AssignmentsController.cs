namespace DesignAndBuilding.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Services;
    using DesignAndBuilding.Web.ViewModels.Assignment;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class AssignmentsController : Controller
    {
        private readonly IAssignmentsService assignmentsService;

        public AssignmentsController(IAssignmentsService assignmentsService)
        {
            this.assignmentsService = assignmentsService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create(int buildingId)
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(AssignmentInputModel assignment)
        {
            await this.assignmentsService.CreateAssignmentAsync(assignment.Description, assignment.EndDate, assignment.DesignerType, decimal.Parse(assignment.BasePricePerSquareMeter), assignment.BuildingId);
            return this.RedirectToAction("Details", "Buildings", new { id = assignment.BuildingId });
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var assignment = await this.assignmentsService.GetAssignmentById(id);

            return this.View(new AssignmentViewModel()
            {
                BasePricePerSquareMeter = assignment.BasePricePerSquareMeter,
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
                    }).ToList(),
            });
        }
    }
}
