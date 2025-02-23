using BookMovieCatalog.Data;
using BookMovieCatalog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace BookMovieCatalog.Controllers
{
    [Authorize]
    public class ReviewLikeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewLikeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Like(int reviewId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "Не сте влезли в профила си." });
            }

            try
            {
                var existingLike = _context.ReviewLikes
                    .AsNoTracking()
                    .FirstOrDefault(l => l.ReviewId == reviewId && l.UserId == userId);

                if (existingLike != null)
                {
                    _context.ReviewLikes.Remove(existingLike);
                }
                else
                {
                    if (!_context.ReviewLikes.Any(l => l.ReviewId == reviewId && l.UserId == userId))
                    {
                        _context.ReviewLikes.Add(new ReviewLike { ReviewId = reviewId, UserId = userId });
                    }
                }

                _context.SaveChanges();
                var likeCount = _context.ReviewLikes.Count(l => l.ReviewId == reviewId);

                
                return Json(new { success = true, likeCount });
            }
            catch (DbUpdateException)
            {
                return Json(new { success = false, message = "Грешка при запис. Опитайте отново." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Възникна проблем. Моля, презаредете страницата." });
            }
        }
    }
}
