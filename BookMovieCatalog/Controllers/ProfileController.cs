using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookMovieCatalog.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

       
        //  форма за AvatarUrl
        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            // Взима текущия user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found or not logged in.");
            }

            // Зарежда всички claims
            var claims = await _userManager.GetClaimsAsync(user);

            // Търси claim със Type = "avatar_url"
            var avatarClaim = claims.FirstOrDefault(c => c.Type == "avatar_url");
            var avatarUrl = avatarClaim?.Value ?? "";

            ViewBag.AvatarUrl = avatarUrl;
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> MyProfile(string avatarUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found or not logged in.");
            }

            // Всички claims
            var claims = await _userManager.GetClaimsAsync(user);
            
            var avatarClaim = claims.FirstOrDefault(c => c.Type == "avatar_url");

            // Заменяме или добавяме claim
            if (avatarClaim != null)
            {
                await _userManager.ReplaceClaimAsync(
                    user,
                    avatarClaim,
                    new Claim("avatar_url", avatarUrl ?? "")
                );
            }
            else
            {
                await _userManager.AddClaimAsync(
                    user,
                    new Claim("avatar_url", avatarUrl ?? "")
                );
            }

            TempData["Message"] = "Avatar updated successfully!";
            return RedirectToAction("MyProfile");
        }
    }
}
