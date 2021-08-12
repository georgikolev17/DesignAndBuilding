namespace DesignAndBuilding.Tests.Mocks
{
    using DesignAndBuilding.Tests.Controllers;
    using global::DesignAndBuilding.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class UserManagerMock : UserManager<ApplicationUser>
    {
        /*public static UserManager<ApplicationUser> Instance
        {
            *//*get
            {
                *//*var mock = new Mock<UserManager<ApplicationUser>>();
                var db = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>());
                var UserManager = new UserManager <ApplicationUser>(new UserStore<ApplicationUser, CustomRole, string, CustomUserLogin, CustomUserRole, CustomUserClaim>(new myDbContext()));*//*



                mock.Setup(dtp => dtp.GetUserAsync(new ClaimsPrincipal())).ReturnsAsync(new ApplicationUser() { DesignerType = DesignerType.ElectroEngineer });

                return mock.Object;*//*
            }*/

            public UserManagerMock(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor,
             IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators,
              IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer,
               IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger)
                : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
            {
            }
        public override async Task<ApplicationUser> GetUserAsync(ClaimsPrincipal principal)
        {
            var user = new ApplicationUser()
            {
                DesignerType = DesignerType.ElectroEngineer,
            };

            user.Id = ControllerConstants.UserId;

            return user;
        }
    }
}
