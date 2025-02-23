using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookMovieCatalog.Models;

namespace BookMovieCatalog.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Favorite> Favorites { get; set; } = null!;
        public DbSet<ReviewLike> ReviewLikes { get; set; } = null!;

        //Таблица за потребителски профили
        public DbSet<UserProfile> UserProfiles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // първо викаме `base.OnModelCreating()`

            //Конфигурация за Favorite
            modelBuilder.Entity<Favorite>()
                .HasKey(f => new { f.UserId, f.BookId, f.MovieId });

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Book)
                .WithMany(b => b.Favorites)
                .HasForeignKey(f => f.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Movie)
                .WithMany(m => m.Favorites)
                .HasForeignKey(f => f.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            // Конфигурация за Review
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Book)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Movie)
                .WithMany(m => m.Reviews)
                .HasForeignKey(r => r.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Конфигурация за ReviewLike
            modelBuilder.Entity<ReviewLike>()
                .HasKey(rl => new { rl.ReviewId, rl.UserId });

            modelBuilder.Entity<ReviewLike>()
                .HasOne(rl => rl.Review)
                .WithMany(r => r.ReviewLikes)
                .HasForeignKey(rl => rl.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);

            //Добавяне на рейтинг статистика
            modelBuilder.Entity<Book>()
                .Property(b => b.TotalVotes)
                .HasDefaultValue(0);

            modelBuilder.Entity<Book>()
                .Property(b => b.SumOfRatings)
                .HasDefaultValue(0);

            modelBuilder.Entity<Movie>()
                .Property(m => m.TotalVotes)
                .HasDefaultValue(0);

            modelBuilder.Entity<Movie>()
                .Property(m => m.SumOfRatings)
                .HasDefaultValue(0);

            // Добавяме конфигурация за UserProfile
            // Ключът е Id (int)
            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(up => up.Id);

                // Свързваме UserProfile.UserId към IdentityUser.Id 
                entity.HasOne<IdentityUser>()
                      .WithOne() 
                      .HasForeignKey<UserProfile>(up => up.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            //админ
            string adminRoleId = "admin-role-12345";
            string adminUserId = "admin-user-12345";

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" }
            );

            var adminUser = new IdentityUser
            {
                Id = adminUserId,
                UserName = "admin@example.com",
                NormalizedUserName = "ADMIN@EXAMPLE.COM",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = System.Guid.NewGuid().ToString()
            };

            PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");

            modelBuilder.Entity<IdentityUser>().HasData(adminUser);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = adminRoleId, UserId = adminUserId }
            );
        }
    }
}
