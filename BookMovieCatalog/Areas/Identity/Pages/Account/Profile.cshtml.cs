using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Threading.Tasks;

namespace BookMovieCatalog.Areas.Identity.Pages.Account
{
    public class ProfileModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ProfileModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public string? Name { get; set; } // Добавено "?" за nullable

        [BindProperty]
        public string? Email { get; set; } // Добавено "?" за nullable

        [BindProperty]
        public IFormFile? AvatarFile { get; set; } // Добавено "?" за nullable

        public string? AvatarPath { get; set; } // Добавено "?" за nullable

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Потребителят не е намерен.");
            }

            Name = user.UserName;
            Email = user.Email;
            AvatarPath = "/avatars/default-avatar.png"; // Можете да зададете динамичен път, ако имате база данни.

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Потребителят не е намерен.");
            }

            if (!string.IsNullOrEmpty(Name))
            {
                user.UserName = Name;
            }

            if (!string.IsNullOrEmpty(Email))
            {
                user.Email = Email;
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            // Актуализиране на аватара
            if (AvatarFile != null)
            {
                try
                {
                    var directoryPath = Path.Combine("wwwroot", "avatars");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    var fileName = $"{user.Id}{Path.GetExtension(AvatarFile.FileName)}";
                    var filePath = Path.Combine(directoryPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await AvatarFile.CopyToAsync(stream);
                    }

                    AvatarPath = $"/avatars/{fileName}";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Възникна грешка при качването на аватара: {ex.Message}");
                    return Page();
                }
            }

            await _signInManager.RefreshSignInAsync(user);

            ViewData["SuccessMessage"] = "Профилът беше успешно актуализиран!";
            return Page();
        }
    }
}
