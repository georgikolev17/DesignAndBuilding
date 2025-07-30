namespace DesignAndBuilding.Web.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;

    // [Authorize]
    public class BidsHub(IAssignmentsService assignmentsService, IUsersService usersService, UserManager<ApplicationUser> userManager) : Hub
    {
        private readonly IAssignmentsService assignmentsService = assignmentsService;
        private readonly IUsersService usersService = usersService;
        private readonly UserManager<ApplicationUser> userManager = userManager;

        public void NewBid(string assignmentId, string userId, decimal price)
        {
            var user = this.usersService.GetUserById(userId);
            var userFullName = user?.FirstName ?? "Ivan" + " " + user?.LastName ?? "Testov";
            this.Clients.Group(assignmentId).SendAsync("newbidplaced", new { userFullName, price, DateTime.Now, assignmentId });
        }

        public override async Task OnConnectedAsync()
        {
            var assignmentsIds = await this.GetUserAssignmentIds();

            foreach (int id in assignmentsIds)
            {
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, id.ToString());
            }

            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var assignmentsIds = await this.GetUserAssignmentIds();

            foreach (int id in assignmentsIds)
            {
                await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, id.ToString());
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Returns all assignments where user has placed bid
        private async Task<ICollection<int>> GetUserAssignmentIds()
        {
            var user = await this.userManager.GetUserAsync(this.Context.User);

            // IDs of assignments where user has placed bid. Used for groups creation
            return this.assignmentsService.GetAssignmentsWhereUserPlacedBid(user.Id).Select(x => x.Id).ToList();
        }
    }
}
