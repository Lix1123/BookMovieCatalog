using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BookMovieCatalog.Controllers;
using BookMovieCatalog.Data;
using BookMovieCatalog.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace BookMovieCatalog.Tests
{
    public class ReviewControllerTests
    {
        private static ApplicationDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        private static Mock<UserManager<IdentityUser>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            return new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public async Task CreateForBook_ValidReview_ReturnsRedirectToAction()
        {
            // Arrange
            var dbContext = GetDatabaseContext();
            var userManagerMock = GetMockUserManager();
            var reviewController = new ReviewController(dbContext, userManagerMock.Object);

            var book = new Book { Id = 1, Title = "Test Book", ImageUrl = "test.jpg" };
            dbContext.Books.Add(book);
            await dbContext.SaveChangesAsync();

            var review = new Review
            {
                Comment = "Great book!",
                Rating = 8,
                BookId = book.Id,
                UserId = "test-user-id",
                Date = DateTime.UtcNow 
            };

            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, review.UserId ?? throw new ArgumentNullException(nameof(review.UserId))) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            reviewController.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = claimsPrincipal } };

            // Act
            var result = await reviewController.CreateForBook(review);

            // Assert
            Assert.NotNull(result);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirectResult.ActionName);
            Assert.Equal("Book", redirectResult.ControllerName);
            Assert.NotNull(redirectResult.RouteValues);
            Assert.Equal(book.Id, redirectResult.RouteValues["id"]);
        }
    }

    public class AdminControllerTests
    {
        private static Mock<UserManager<IdentityUser>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            return new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private static ApplicationDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task DeleteUser_ValidUser_ReturnsRedirectToAction()
        {
            // Arrange
            var dbContext = GetDatabaseContext();
            var userManagerMock = GetMockUserManager();
            var adminController = new AdminController(dbContext, userManagerMock.Object);

            var user = new IdentityUser { Id = "test-user-id", UserName = "TestUser" };
            userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<IdentityUser>())).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await adminController.DeleteUser("test-user-id");

            // Assert
            Assert.NotNull(result);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Users", redirectResult.ActionName);
        }
    }
}
