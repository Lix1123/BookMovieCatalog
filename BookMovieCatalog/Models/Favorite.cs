using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BookMovieCatalog.Models
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!; 

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; } = null!;

        public int? BookId { get; set; }
        public Book? Book { get; set; }

        public int? MovieId { get; set; }
        public Movie? Movie { get; set; }
    }
}
