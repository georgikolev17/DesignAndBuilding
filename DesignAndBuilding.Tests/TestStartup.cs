namespace DesignAndBuilding.Tests
{
    using DesignAndBuilding.Tests.Mocks;
    using global::DesignAndBuilding.Data.Models;
    using global::DesignAndBuilding.Web;
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

            services.ReplaceSingleton<UserManager<ApplicationUser>, UserManagerMock>();
        }
    }
}
