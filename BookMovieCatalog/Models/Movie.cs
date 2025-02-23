using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMovieCatalog.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Заглавието не може да бъде по-дълго от 100 символа.")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(50, ErrorMessage = "Името на режисьора не може да бъде по-дълго от 50 символа.")]
        public string Director { get; set; } = string.Empty;

        [Required]
        public string Genre { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime YearReleased { get; set; }

        [StringLength(500, ErrorMessage = "Описанието не може да бъде по-дълго от 500 символа.")]
        public string Description { get; set; } = string.Empty;

        [Url(ErrorMessage = "Моля, въведете валиден URL за изображението.")]
        public string? ImageUrl { get; set; } = "https://via.placeholder.com/150";

        [Url(ErrorMessage = "Моля, въведете валиден URL за трейлъра.")]
        public string? TrailerUrl { get; set; } = string.Empty;

        public List<Review> Reviews { get; set; } = new List<Review>();

        public List<Favorite> Favorites { get; set; } = new List<Favorite>(); // 🔹 Добавено

        public int TotalVotes { get; set; } = 0;
        public int SumOfRatings { get; set; } = 0;

        [NotMapped]
        public double AverageRating => TotalVotes == 0 ? 0 : Math.Round((double)SumOfRatings / TotalVotes, 1);
    }
}
