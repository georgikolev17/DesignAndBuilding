namespace DesignAndBuilding.Tests
{
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Services;
    using DesignAndBuilding.Tests.Mocks;
    using DesignAndBuilding.Web;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using MyTested.AspNetCore.Mvc;

    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) 
            : base(configuration)
        {
        }

        public void ConfigureTestServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.ReplaceDbContext();

            services.ReplaceTransient<IAssignmentsService>(_ => AssignmentsServiceMock.Instance);
            //services.ReplaceTransient<UserManager<ApplicationUser>>(_ => UserManagerMock.Instance);
            services.ReplaceSingleton<UserManager<ApplicationUser>, UserManagerMock>();
        }
    }
}
