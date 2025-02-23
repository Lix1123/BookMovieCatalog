using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMovieCatalog.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Коментарът не може да надвишава 1000 символа.")]
        public string Comment { get; set; } = string.Empty;

        [Required]
        [Range(1, 10, ErrorMessage = "Рейтингът трябва да бъде между 1 и 10.")]
        public int Rating { get; set; }

        public DateTime Date { get; set; } = DateTime.Now; 

        //  Свързване с книга (ако е рецензия за книга)
        public int? BookId { get; set; }
        [ForeignKey("BookId")]
        public Book? Book { get; set; }

        //  Свързване с филм (ако е рецензия за филм)
        public int? MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movie? Movie { get; set; }

        //  Свързване с потребителя, написал рецензията
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }

        public string? UserName { get; set; }

        // харесвания 
        public ICollection<ReviewLike> ReviewLikes { get; set; } = new List<ReviewLike>();
    }
}
