using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BookMovieCatalog.Models
{
    public class ReviewLike
    {
        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public int ReviewId { get; set; }

        [ForeignKey("UserId")]
        public virtual IdentityUser User { get; set; } = null!;

        [ForeignKey("ReviewId")]
        public virtual Review Review { get; set; } = null!;
    }
}
