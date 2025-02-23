using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMovieCatalog.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string Genre { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime YearPublished { get; set; }

        public string Description { get; set; } = string.Empty;

        public string? ImageUrl { get; set; } = "https://via.placeholder.com/150";

        public List<Review> Reviews { get; set; } = new List<Review>();

        public List<Favorite> Favorites { get; set; } = new List<Favorite>(); // 🔹 Добавено

        public int TotalVotes { get; set; } = 0;
        public int SumOfRatings { get; set; } = 0;

        [NotMapped]
        public double AverageRating => TotalVotes == 0 ? 0 : Math.Round((double)SumOfRatings / TotalVotes, 1);
    }
}
