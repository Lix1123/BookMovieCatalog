using Microsoft.AspNetCore.Mvc;
using BookMovieCatalog.Data;
using BookMovieCatalog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BookMovieCatalog.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReviewController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var reviews = await _context.Reviews
                .Include(r => r.Book)
                .Include(r => r.Movie)
                .AsNoTracking()
                .ToListAsync();

            var users = await _userManager.Users.ToListAsync();
            var avatars = new Dictionary<string, string>();

            foreach (var user in users)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                var avatarClaim = claims.FirstOrDefault(c => c.Type == "avatar_url");
                avatars[user.Id] = avatarClaim?.Value ?? "/images/default-avatar.png";
            }

            ViewBag.UserAvatars = avatars;
            ViewBag.Books = await _context.Books.AsNoTracking().ToListAsync();
            ViewBag.Movies = await _context.Movies.AsNoTracking().ToListAsync();

            return View(reviews);
        }

        [Authorize]
        public async Task<IActionResult> CreateForBook(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null) return NotFound("Книгата не е намерена.");

            ViewBag.BookTitle = book.Title;
            ViewBag.BookId = bookId;
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateForBook(Review review)
        {
            if (!ModelState.IsValid)
                return View(review);

            review.Date = DateTime.UtcNow;
            review.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            review.UserName = User.Identity?.Name ?? "Анонимен";

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            await UpdateBookRating(review.BookId ?? 0); 

            return RedirectToAction("Details", "Book", new { id = review.BookId });
        }

        [Authorize]
        public async Task<IActionResult> CreateForMovie(int movieId)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null) return NotFound("Филмът не е намерен.");

            ViewBag.MovieTitle = movie.Title;
            ViewBag.MovieId = movieId;
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateForMovie(Review review)
        {
            if (!ModelState.IsValid)
                return View(review);

            review.Date = DateTime.UtcNow;
            review.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            review.UserName = User.Identity?.Name ?? "Анонимен";

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            await UpdateMovieRating(review.MovieId ?? 0); 

            return RedirectToAction("Details", "Movie", new { id = review.MovieId });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Review review)
        {
            if (!ModelState.IsValid)
                return View(review);

            var existingReview = await _context.Reviews.AsNoTracking().FirstOrDefaultAsync(r => r.Id == review.Id);
            if (existingReview == null) return NotFound("Рецензията не е намерена.");

            if (existingReview.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
                return Forbid();

            review.Date = DateTime.UtcNow;
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();

            if (review.BookId.HasValue)
            {
                await UpdateBookRating(review.BookId.Value);
                return RedirectToAction("Details", "Book", new { id = review.BookId });
            }
            else if (review.MovieId.HasValue)
            {
                await UpdateMovieRating(review.MovieId.Value);
                return RedirectToAction("Details", "Movie", new { id = review.MovieId });
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return NotFound("Рецензията не е намерена.");

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            if (review.BookId.HasValue)
            {
                await UpdateBookRating(review.BookId.Value);
                return RedirectToAction("Details", "Book", new { id = review.BookId });
            }
            else if (review.MovieId.HasValue)
            {
                await UpdateMovieRating(review.MovieId.Value);
                return RedirectToAction("Details", "Movie", new { id = review.MovieId });
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task UpdateBookRating(int bookId)
        {
            var book = await _context.Books.Include(b => b.Reviews).FirstOrDefaultAsync(b => b.Id == bookId);
            if (book == null) return;

            book.TotalVotes = book.Reviews.Count;
            book.SumOfRatings = book.Reviews.Sum(r => r.Rating);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Логване на грешката
            }
        }

        private async Task UpdateMovieRating(int movieId)
        {
            var movie = await _context.Movies.Include(m => m.Reviews).FirstOrDefaultAsync(m => m.Id == movieId);
            if (movie == null) return;

            movie.TotalVotes = movie.Reviews.Count;
            movie.SumOfRatings = movie.Reviews.Sum(r => r.Rating );

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Логване на грешката
            }
        }
    }
}
