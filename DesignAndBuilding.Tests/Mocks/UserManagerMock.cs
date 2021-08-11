namespace DesignAndBuilding.Tests.Mocks
{
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Services;
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class UserManagerMock
    {
        public static UserManager<ApplicationUser> Instance
        {
            get
            {
                var mock = new Mock<UserManager<ApplicationUser> >();

                mock.Setup(dtp => dtp.GetUserAsync(new ClaimsPrincipal())).ReturnsAsync(new ApplicationUser() { DesignerType = DesignerType.ElectroEngineer });

                return mock.Object;
            }
        }
    }
}
