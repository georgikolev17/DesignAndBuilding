namespace DesignAndBuilding.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using DesignAndBuilding.Common;
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Services;
    using DesignAndBuilding.Web.ViewModels;
    using DesignAndBuilding.Web.ViewModels.Assignment;
    using DesignAndBuilding.Web.ViewModels.Bid;
    using Dropbox.Api;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

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
                return this.View(assignment);
            }

            await this.assignmentsService.CreateAssignmentAsync(assignment.Description.ToList(), assignment.EndDate, assignment.DesignerType, assignment.BuildingId);
            return this.RedirectToAction("Details", "Buildings", new { id = assignment.BuildingId });
        }

        public async Task<IActionResult> Details(int id)
        {
            var assignment = await this.assignmentsService.GetAssignmentById(id);
            var user = await this.userManager.GetUserAsync(this.User);

            if (assignment == null)
            {
                return this.NotFound();
            }

            return this.View(new AssignmentViewModel()
            {
                AssignmentId = id,
                CreatedOn = assignment.CreatedOn,
                DesignerType = assignment.DesignerType,
                EndDate = assignment.EndDate,
                IsFinished = assignment.IsFinished,
                HasUserCreatedAssignment = this.assignmentsService.HasUserCreatedAssignment(user.Id, assignment.Id),
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
                        UserFullName = x.Designer.FirstName + " " + x.Designer.LastName,
                        PhoneNumber = x.Designer.PhoneNumber,
                        Email = x.Designer.Email,
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

            if (assignment == null)
            {
                return this.NotFound();
            }

            if (user.DesignerType != assignment.DesignerType)
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = $"Само {DisplayDesignertypeInBulgarian(assignment.DesignerType)}и могат да наддават за това задание!" });
            }

            if (!this.ModelState.IsValid)
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = $"Невалидно наддаване" });
            }

            await this.bidsService.CreateBidAsync(user.Id, bidViewModel.Id, bidViewModel.BidPrice);

            var usersToSendNotification = this.assignmentsService.GetAllUsersBidInAssignment(assignment.Id);

            // Assignment creator should also recieve notification
            usersToSendNotification.Add(assignment.Building.ArchitectId);

            // User who placed bid will recieve another notification
            usersToSendNotification.Remove(user.Id);
            var userPlacedBid = new List<string>() { user.Id };
            await this.notificationsService.AddNotificationAsync(userPlacedBid, $"Вие наддадохте с {bidViewModel.BidPrice} лв. за {assignment.Building.Name}!");

            // All other users placed bids for this assignment will recieve this notification
            await this.notificationsService.AddNotificationAsync(usersToSendNotification, $"Има ново наддаване в {assignment.Building.Name} - {bidViewModel.BidPrice} лв.");

            return this.Redirect($"/assignments/details/{bidViewModel.Id}");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var assignment = await this.assignmentsService.GetAssignmentById(id);

            if (assignment == null)
            {
                return this.NotFound();
            }

            if (!this.assignmentsService.HasUserCreatedAssignment(user.Id, id) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал заданието, може да го редактира" });
            }

            var files = new List<IFormFile>();

            foreach (var file in assignment.Description)
            {
                var stream = new MemoryStream(file.Content);
                files.Add(new FormFile(stream, 0, file.Content.Length, file.Name, file.Name));
            }

            var assignmentViewModel = new AssignmentInputModel()
            {
                BuildingId = assignment.BuildingId,

                // TODO: Fix desccription display
                Description = files,
                DesignerType = assignment.DesignerType,
                EndDate = assignment.EndDate,
            };

            return this.View(assignmentViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AssignmentInputModel viewModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var assignment = await this.assignmentsService.GetAssignmentById(id);

            if (assignment == null)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid || viewModel.EndDate < DateTime.Now)
            {
                return this.View(viewModel);
            }

            if (!this.assignmentsService.HasUserCreatedAssignment(user.Id, id) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал заданието, може да го редактира" });
            }

            await this.assignmentsService.EditAssignment(viewModel.DesignerType, viewModel.Description.ToList(), viewModel.EndDate, id);

            return this.Redirect("/");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var assignment = await this.assignmentsService.GetAssignmentById(id);

            if (assignment == null)
            {
                return this.NotFound();
            }

            if (!this.assignmentsService.HasUserCreatedAssignment(user.Id, id) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал заданието, може да го изтрие!" });
            }

            await this.assignmentsService.RemoveAssignment(id);

            return this.Redirect($"/buildings/details/{assignment.BuildingId}");
        }

        private static string DisplayDesignertypeInBulgarian(DesignerType designerType)
        {
            switch (designerType)
            {
                case DesignerType.Other:
                    return "друг";
                case DesignerType.Architect:
                    return "архитект";
                case DesignerType.BuildingConstructionEngineer:
                    return "строителни конструктор";
                case DesignerType.ElectroEngineer:
                    return "електро инженер";
                case DesignerType.PlumbingEngineer:
                    return "ВиК инженер";
                case DesignerType.HVACEngineer:
                    return "ОВК инженер";
                default:
                    return "друг";
            }
        }

        public async Task<IActionResult> Download(int id)
        {
            var files = this.assignmentsService.GetFilesForAssignment(id).ToList();

            using (var compressedFileStream = new MemoryStream())
            {
                // Create an archive and store the stream in memory.
                using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Create, false))
                {
                    foreach (var file in files)
                    {
                        // Create a zip entry for each attachment
                        var zipEntry = zipArchive.CreateEntry(file.Name);

                        // Get the stream of the attachment
                        using (var originalFileStream = new MemoryStream(file.Content))
                        using (var zipEntryStream = zipEntry.Open())
                        {
                            // Copy the attachment stream to the zip entry stream
                            originalFileStream.CopyTo(zipEntryStream);
                        }
                    }
                }

                return new FileContentResult(compressedFileStream.ToArray(), "application/zip") { FileDownloadName = "Description.zip" };
            }
        }
    }
}
