namespace DesignAndBuilding.Web.Areas.Administration.Controllers
{
    using DesignAndBuilding.Common;
    using DesignAndBuilding.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
