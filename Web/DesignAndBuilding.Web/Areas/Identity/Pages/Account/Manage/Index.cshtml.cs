using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DesignAndBuilding.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DesignAndBuilding.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool CanChangeUserType { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Специалност")]
            public UserType UserType { get; set; }

            [Phone]
            [Display(Name = "Телефон")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            CanChangeUserType = user.UserType == UserType.Other;

            Input = new InputModel
            {
                UserType = user.UserType,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Неочаквана грешка при опит за промяна на телефонния номер.";
                    return RedirectToPage();
                }
            }

            // Handle UserType change
            if (Input.UserType != user.UserType)
            {
                if (user.UserType == UserType.Other)
                {
                    user.UserType = Input.UserType;
                    var updateResult = await _userManager.UpdateAsync(user);
                    if (!updateResult.Succeeded)
                    {
                        StatusMessage = "Неочаквана грешка при опит за промяна на специалността.";
                        return RedirectToPage();
                    }
                    StatusMessage = "Вашият профил е актуализиран успешно. Специалността е променена и вече не може да бъде редактирана.";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Специалността не може да бъде променена след като е зададена.");
                    await LoadAsync(user);
                    return Page();
                }
            }
            else
            {
                StatusMessage = "Вашият профил е актуализиран успешно.";
            }

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToPage();
        }
    }
}
