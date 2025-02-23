#nullable disable

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace BookMovieCatalog.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
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
            public string PhoneNumber { get; set; }
            public string AvatarUrl { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string AboutMe { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            Username = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                AvatarUrl = GetClaimValue(claims, "avatar_url"),
                FirstName = GetClaimValue(claims, "first_name"),
                LastName = GetClaimValue(claims, "last_name"),
                AboutMe = GetClaimValue(claims, "about_me")
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("Unable to load user.");

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("Unable to load user.");

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
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            var claims = await _userManager.GetClaimsAsync(user);
            await UpdateClaimAsync(user, claims, "avatar_url", Input.AvatarUrl);
            await UpdateClaimAsync(user, claims, "first_name", Input.FirstName);
            await UpdateClaimAsync(user, claims, "last_name", Input.LastName);
            await UpdateClaimAsync(user, claims, "about_me", Input.AboutMe);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        private async Task UpdateClaimAsync(IdentityUser user, IList<Claim> existingClaims, string claimType, string newValue)
        {
            var oldClaim = existingClaims.FirstOrDefault(c => c.Type == claimType);
            if (oldClaim != null)
            {
                if (oldClaim.Value != (newValue ?? ""))
                {
                    await _userManager.ReplaceClaimAsync(user, oldClaim, new Claim(claimType, newValue ?? ""));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(newValue))
                {
                    await _userManager.AddClaimAsync(user, new Claim(claimType, newValue));
                }
            }
        }

        private string GetClaimValue(IList<Claim> claims, string claimType)
        {
            return claims.FirstOrDefault(c => c.Type == claimType)?.Value ?? "";
        }
    }
}
