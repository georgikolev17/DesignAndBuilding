namespace DesignAndBuilding.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Common;
    using DesignAndBuilding.Services;
    using DesignAndBuilding.Web.Controllers;
    using DesignAndBuilding.Web.ViewModels.Administration.Dashboard;
    using DesignAndBuilding.Web.ViewModels.Building;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class DashboardController : BaseController
    {
        private readonly IMemoryCache cache;
        private readonly IAssignmentsService assignmentsService;
        private readonly IBuildingsService buildingsService;
        private readonly IUsersService usersService;

        public DashboardController(IMemoryCache cache, IAssignmentsService assignmentsService, IBuildingsService buildingsService, IUsersService usersService)
        {
            this.cache = cache;
            this.assignmentsService = assignmentsService;
            this.buildingsService = buildingsService;
            this.usersService = usersService;
        }

        public IActionResult Index()
        {
            const string statisticsCacheKey = "StatisticsKey";

            var statistics = this.cache.Get<IndexViewModel>("StatisticsKey");

            if (statistics == null)
            {
                statistics = new IndexViewModel()
                {
                    AssignmentsCount = this.assignmentsService.GetAssignmentsCount(),
                    BuildingsCount = this.buildingsService.GetBuildingsCount(),
                    UsersCount = this.usersService.GetUsersCount(),
                };

                var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

                this.cache.Set(statisticsCacheKey, statistics, cacheOptions);
            }

            return this.View(statistics);
        }

        public async Task<IActionResult> AllBuildings()
        {
            var buildings = await this.buildingsService.GetAllBuildings();

            var viewModel = buildings.Select(x => new MyBuildingsViewModel()
            {
                Id = x.Id,
                BuildingType = x.BuildingType,
                Name = x.Name,
                TotalBuildUpArea = x.TotalBuildUpArea,
            }).ToList();

            return this.View(viewModel);
        }
    }
}
