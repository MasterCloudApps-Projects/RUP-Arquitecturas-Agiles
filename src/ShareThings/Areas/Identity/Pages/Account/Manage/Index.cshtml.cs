using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShareThings.Areas.Identity.Data;

namespace ShareThings.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ShareThingsUser> _userManager;
        private readonly SignInManager<ShareThingsUser> _signInManager;

        public IndexModel(
            UserManager<ShareThingsUser> userManager,
            SignInManager<ShareThingsUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [DataType(DataType.Text)]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "UserName")]
            public string UserName { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Surname")]
            public string Surname { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Address")]
            public string Address { get; set; }

            [DataType(DataType.PostalCode)]
            [Display(Name = "PostalCode")]
            public string PostalCode { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Country")]
            public string Country { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "BirthDate")]
            public DateTime BirthDate { get; set; }


            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }


            //[EmailAddress]
            //[Display(Name = "Email")]
            //public string Email { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

        }

        private async Task LoadAsync(ShareThingsUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                UserName = Username,
                //Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                Address = user.Address,
                PostalCode = user.PostalCode,
                Country = user.Country,
                PhoneNumber = phoneNumber,
                BirthDate = user.BirthDate
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

            if (Input.Name != user.Name)
                user.Name = Input.Name;

            if (Input.Surname != user.Surname)
                user.Surname = Input.Surname;

            if (Input.PostalCode != user.PostalCode)
                user.PostalCode = Input.PostalCode;

            if (Input.Address != user.Address)
                user.Address = Input.Address;

            if (Input.Country != user.Country)
                user.Country = Input.Country;

            if (Input.BirthDate != user.BirthDate)
                user.BirthDate = Input.BirthDate;


            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
