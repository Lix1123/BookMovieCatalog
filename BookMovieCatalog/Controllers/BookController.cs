using Microsoft.AspNetCore.Mvc;
using BookMovieCatalog.Data;
using BookMovieCatalog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace BookMovieCatalog.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static readonly List<string> _genres = new() { "Фантастика", "Криминале", "Роман", "Екшън" };

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Списък с книги + търсене
        public IActionResult Index(string search)
        {
            var books = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                books = books.Where(b => b.Title.ToLower().Contains(search) || b.Author.ToLower().Contains(search));
            }

            ViewBag.SearchQuery = search;
            return View(books.ToList());
        }

        // Топ 10 книги
        public IActionResult TopBooks()
        {
            var topBooks = _context.Books
                .Include(b => b.Reviews) // Зареждаме рецензиите, за да се изчисли рейтингът
                .AsEnumerable() 
                .OrderByDescending(b => b.AverageRating)
                .Take(10)
                .ToList();

            return View(topBooks);
        }

        //Детайли за книга + рецензии
        public IActionResult Details(int id)
        {
            var book = _context.Books
                .Include(b => b.Reviews)
                .FirstOrDefault(b => b.Id == id);

            if (book == null) return NotFound("Книгата не беше намерена.");

            ViewBag.AverageRating = book.AverageRating;
            return View(book);
        }

        //Добавяне на книга 
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Genres = _genres;
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (!ModelState.IsValid) return View(book);

            _context.Books.Add(book);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // Редактиране на книга 
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null) return NotFound("Книгата не беше намерена.");

            ViewBag.Genres = _genres;
            return View(book);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (!ModelState.IsValid) return View(book);

            _context.Books.Update(book);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // Изтриване на книга 
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null) return NotFound("Книгата не беше намерена.");

            return View(book);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null) return NotFound("Книгата не беше намерена.");

            _context.Books.Remove(book);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        //Обновяване на рейтинг при нови рецензии
        private void UpdateBookRating(int bookId)
        {
            var book = _context.Books
                .Include(b => b.Reviews)
                .FirstOrDefault(b => b.Id == bookId);

            if (book == null) return;

            book.TotalVotes = book.Reviews.Count;
            book.SumOfRatings = book.Reviews.Sum(r => r.Rating);

            _context.SaveChanges();
        }
    }
}
