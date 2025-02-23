using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookMovieCatalog.Data;
using BookMovieCatalog.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookMovieCatalog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound("Невалиден ID.");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Потребителят не е намерен.");

            await _userManager.DeleteAsync(user);
            return RedirectToAction("Users");
        }

        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound("Невалиден ID.");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Потребителят не е намерен.");

            var model = new EditUserViewModel
            {
                Id = user.Id ?? string.Empty,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return NotFound("Потребителят не е намерен.");

            user.Email = model.Email ?? string.Empty;
            user.PhoneNumber = model.PhoneNumber ?? string.Empty;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Грешка при запазване.");
                return View(model);
            }

            return RedirectToAction("Users");
        }

        public async Task<IActionResult> Movies()
        {
            var movies = await _context.Movies.ToListAsync();
            return View(movies);
        }

        public async Task<IActionResult> EditMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();

            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> EditMovie(int id, Movie movie)
        {
            if (!ModelState.IsValid)
                return View(movie);

            var existingMovie = await _context.Movies.FindAsync(id);
            if (existingMovie == null)
                return NotFound();

            existingMovie.Title = movie.Title ?? existingMovie.Title;
            existingMovie.Director = movie.Director ?? existingMovie.Director;
            existingMovie.Genre = movie.Genre ?? existingMovie.Genre;
            existingMovie.YearReleased = movie.YearReleased;
            existingMovie.ImageUrl = movie.ImageUrl ?? existingMovie.ImageUrl;

            _context.Update(existingMovie);
            await _context.SaveChangesAsync();
            return RedirectToAction("Movies");
        }

        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Movies");
        }

        public async Task<IActionResult> Books()
        {
            var books = await _context.Books.ToListAsync();
            return View(books);
        }

        public async Task<IActionResult> EditBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> EditBook(int id, Book book)
        {
            if (!ModelState.IsValid)
                return View(book);

            var existingBook = await _context.Books.FindAsync(id);
            if (existingBook == null)
                return NotFound();

            existingBook.Title = book.Title ?? existingBook.Title;
            existingBook.Author = book.Author ?? existingBook.Author;
            existingBook.Genre = book.Genre ?? existingBook.Genre;
            existingBook.YearPublished = book.YearPublished;
            existingBook.ImageUrl = book.ImageUrl ?? existingBook.ImageUrl;

            _context.Update(existingBook);
            await _context.SaveChangesAsync();
            return RedirectToAction("Books");
        }

        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Books");
        }

        public async Task<IActionResult> Reviews()
        {
            var reviews = await _context.Reviews
                .Include(r => r.Book)
                .Include(r => r.Movie)
                .ToListAsync();
            return View(reviews);
        }

        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Reviews");
        }

        public async Task<IActionResult> Delete()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
