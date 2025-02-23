using Microsoft.AspNetCore.Mvc;
using BookMovieCatalog.Data;
using BookMovieCatalog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BookMovieCatalog.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static readonly List<string> _genres = new() { "Екшън", "Драма", "Комедия", "Фантастика", "Ужаси" };

        public MovieController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search)
        {
            var movies = _context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                movies = movies.Where(m => m.Title.ToLower().Contains(search) || m.Director.ToLower().Contains(search));
            }

            ViewBag.SearchQuery = search;
            ViewBag.IsAdmin = User.Identity != null && User.IsInRole("Admin"); // ✅ Поправено

            return View(movies.ToList());
        }

        public IActionResult TopMovies()
        {
            var topMovies = _context.Movies
                .Include(m => m.Reviews)
                .AsEnumerable() // Превключване на client-side LINQ
                .OrderByDescending(m => m.AverageRating)
                .Take(10)
                .ToList();

            return View(topMovies);
        }

        public IActionResult Details(int id)
        {
            var movie = _context.Movies
                .Include(m => m.Reviews)
                .FirstOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return NotFound("Филмът не беше намерен.");
            }

            ViewBag.AverageRating = movie.AverageRating;
            return View(movie);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Genres = _genres;
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Genres = _genres;
                return View(movie);
            }

            movie.ImageUrl ??= "https://via.placeholder.com/150";

            _context.Movies.Add(movie);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null)
            {
                return NotFound("Филмът не беше намерен.");
            }

            ViewBag.Genres = _genres;
            return View(movie);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Genres = _genres;
                return View(movie);
            }

            movie.ImageUrl ??= "https://via.placeholder.com/150";

            _context.Movies.Update(movie);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null)
            {
                return NotFound("Филмът не беше намерен.");
            }
            return View(movie);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult AddReview(int movieId)
        {
            var movie = _context.Movies.Find(movieId);
            if (movie == null)
            {
                return NotFound("Филмът не беше намерен.");
            }

            ViewBag.MovieId = movieId;
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddReview(Review review)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MovieId = review.MovieId;
                return View(review);
            }

            review.Date = DateTime.Now;
            review.UserId = User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            review.UserName = User?.Identity?.Name ?? "Анонимен";

            _context.Reviews.Add(review);
            _context.SaveChanges();
            UpdateMovieRating(review.MovieId ?? 0);
            return RedirectToAction("Details", new { id = review.MovieId });
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditReview(Review review)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MovieId = review.MovieId;
                return View(review);
            }

            var existingReview = _context.Reviews.AsNoTracking().FirstOrDefault(r => r.Id == review.Id);
            if (existingReview == null) return NotFound("Рецензията не беше намерена.");

            if (existingReview.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
                return Forbid();

            review.Date = DateTime.Now;
            _context.Reviews.Update(review);
            _context.SaveChanges();
            UpdateMovieRating(review.MovieId ?? 0);

            return RedirectToAction("Details", new { id = review.MovieId });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteReview(int id)
        {
            var review = _context.Reviews.Find(id);
            if (review == null) return NotFound("Рецензията не беше намерена.");

            _context.Reviews.Remove(review);
            _context.SaveChanges();
            UpdateMovieRating(review.MovieId ?? 0);

            return RedirectToAction("Details", new { id = review.MovieId });
        }

        private void UpdateMovieRating(int movieId)
        {
            var movie = _context.Movies.Include(m => m.Reviews).FirstOrDefault(m => m.Id == movieId);
            if (movie == null) return;

            movie.TotalVotes = movie.Reviews.Count;
            movie.SumOfRatings = movie.Reviews.Sum(r => r.Rating);

            _context.SaveChanges();
        }
    }
}
