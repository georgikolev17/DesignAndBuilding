namespace DesignAndBuilding.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using AutoMapper;
    using DesignAndBuilding.Common;
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Services;
    using DesignAndBuilding.Web.Hubs;
    using DesignAndBuilding.Web.ViewModels;
    using DesignAndBuilding.Web.ViewModels.Assignment;
    using DesignAndBuilding.Web.ViewModels.Bid;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.DependencyInjection;

    [Authorize]
    public class AssignmentsController : Controller
    {
        private readonly IAssignmentsService assignmentsService;
        private readonly IBidsService bidsService;
        private readonly INotificationsService notificationsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFilesService filesService;
        private readonly IMapper mapper;

        public AssignmentsController(IAssignmentsService assignmentsService, IBidsService bidsService, INotificationsService notificationsService, UserManager<ApplicationUser> userManager, IFilesService filesService, IMapper mapper)
        {
            this.assignmentsService = assignmentsService;
            this.bidsService = bidsService;
            this.notificationsService = notificationsService;
            this.userManager = userManager;
            this.filesService=filesService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Create()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            // Check if current user is architect or investor
            if (user.UserType != UserType.Architect)
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само архитекти и инвеститори могат да създават задания" });
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AssignmentInputModel assignment)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            // Check if current user is architect or investor
            if (user.UserType != UserType.Architect)
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само архитекти и инвеститори могат да създават задания" });
            }

            if (!this.ModelState.IsValid || assignment.EndDate < DateTime.UtcNow)
            {
                return this.View(assignment);
            }

            await this.assignmentsService.CreateAssignmentAsync(assignment.Description.ToList(), assignment.EndDate, assignment.UserType, assignment.BuildingId, user.UserType);
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

            var viewModel = await this.ProcessAssignmentToViewModel(assignment);

            return this.View(viewModel);
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

            // Check if current user is of correct designer type
            if (user.UserType != assignment.UserType)
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = $"Само {DisplayUserTypeInBulgarian(assignment.UserType)}и могат да наддават за това задание!" });
            }

            if (!this.ModelState.IsValid)
            {
                var viewModel = await this.ProcessAssignmentToViewModel(assignment);
                return this.View(viewModel);
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

            var strId = bidViewModel.Id.ToString();

            // Retrieve the access token for the authenticated user
            var cookie = this.Request.Cookies[".AspNetCore.Identity.Application"];
            System.Net.Cookie cook = new System.Net.Cookie(".AspNetCore.Identity.Application", cookie, "/bidshub") { Domain = this.Request.Host.Host };
            var connection = new HubConnectionBuilder()
                .AddMessagePackProtocol()
                .WithUrl($"https://{this.Request.Host}/bidshub", options =>
                {
                    // options.UseDefaultCredentials = true;
                    /*options.HttpMessageHandlerFactory = _ => new HttpClientHandler
                    {
                        CookieContainer = new System.Net.CookieContainer(),
                        UseCookies = true,
                    };*/
                    options.Headers[".AspNetCore.Identity.Application"] = cookie;
                    options.Cookies.Add(cook);
                })
                .WithAutomaticReconnect()
                .Build();
            await connection.StartAsync();
            await connection.InvokeAsync("NewBid", strId, user.Id, bidViewModel.BidPrice);
            await connection.DisposeAsync();
            // return this.Ok();

            return this.Redirect($"/assignments/details/{bidViewModel.Id}");
        }

        // WARNING: This functionality does not work now. Decide whether to support it.
        public async Task<IActionResult> Edit(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var assignment = await this.assignmentsService.GetAssignmentById(id);

            if (assignment == null)
            {
                return this.NotFound();
            }

            // Check if current user is author of the assignment
            if (!this.assignmentsService.HasUserCreatedAssignment(user.Id, id) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал заданието, може да го редактира" });
            }

            var files = new List<IFormFile>();
            // WARNING: The foreach below is commented because it gives errors. This is done for the sake of compilation. Fix it if this functionality is to be supported.
            //foreach (var file in assignment.Description)
            //{
            //    var stream = new MemoryStream(file.Content);
            //    files.Add(new FormFile(stream, 0, file.Content.Length, file.Name, file.Name));
            //}

            var assignmentViewModel = new AssignmentInputModel()
            {
                BuildingId = assignment.BuildingId,

                Description = files,
                UserType = assignment.UserType,
                EndDate = assignment.EndDate,
            };

            return this.View(assignmentViewModel);
        }


        // WARNING: This functionality does not work now. Decide whether to support it.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, AssignmentInputModel viewModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            // Validate input data
            var assignment = await this.assignmentsService.GetAssignmentById(id);

            if (assignment == null)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid || viewModel.EndDate < DateTime.Now)
            {
                return this.View(viewModel);
            }

            // Check if current user is author of the assignment
            if (!this.assignmentsService.HasUserCreatedAssignment(user.Id, id) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал заданието, може да го редактира" });
            }

            await this.assignmentsService.EditAssignment(viewModel.UserType, viewModel.Description.ToList(), viewModel.EndDate, id);

            return this.Redirect($"/buildings/details/{assignment.BuildingId}");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var assignment = await this.assignmentsService.GetAssignmentById(id);

            if (assignment == null)
            {
                return this.NotFound();
            }

            // Check if current user is author of the assignment
            if (!this.assignmentsService.HasUserCreatedAssignment(user.Id, id) && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.View("Error", new ErrorViewModel() { ErrorMessage = "Само потребителя, създал заданието, може да го изтрие!" });
            }

            await this.assignmentsService.RemoveAssignment(id);

            return this.Redirect($"/buildings/details/{assignment.BuildingId}");
        }

        public async Task<IActionResult> Download(int id)
        {
            var files = await this.filesService.GetDescriptionFilesForAssignmentAsync(id);

            using (var compressedFileStream = new MemoryStream())
            {
                // Create an archive and store the stream in memory.
                using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Create, false))
                {
                    foreach (var file in files)
                    {
                        string fileName = file.Key;
                        Stream fileData = file.Value;

                        //using (var fileStream = System.IO.File.Create(@"D:\TUe\Year-2\Q1\Algebra for Security\tatko original 2.pdf"))
                        //{
                        //    await fileData.CopyToAsync(fileStream);
                        //}

                        // Create a zip entry for each attachment
                        var zipEntry = zipArchive.CreateEntry(fileName);

                        // Get the stream of the attachment
                        using var originalFileStream = new MemoryStream();
                        fileData.CopyTo(originalFileStream);
                        originalFileStream.Position = 0;
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

        // TODO: This is not needed . Remove it.
        private static string DisplayUserTypeInBulgarian(UserType userType)
        {
            switch (userType)
            {
                case UserType.Other:
                    return "друг";
                case UserType.Architect:
                    return "архитект";
                case UserType.BuildingConstructionEngineer:
                    return "строителни конструктор";
                case UserType.ElectroEngineer:
                    return "електро инженер";
                case UserType.PlumbingEngineer:
                    return "ВиК инженер";
                case UserType.HVACEngineer:
                    return "ОВК инженер";
                default:
                    return "друг";
            }
        }

        private async Task<AssignmentViewModel> ProcessAssignmentToViewModel(Assignment assignment)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = this.mapper.Map<AssignmentViewModel>(assignment);

            viewModel.HasUserCreatedAssignment = this.assignmentsService.HasUserCreatedAssignment(user.Id, assignment.Id);
            viewModel.Bids = viewModel.Bids.OrderBy(x => x.Price).ThenByDescending(x => x.TimePlaced).ToList();
            return viewModel;
        }
    }
}
