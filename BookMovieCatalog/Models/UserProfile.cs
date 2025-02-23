using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMovieCatalog.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        // Връзка към AspNetUsers (IdentityUser) по string UserId
        [Required]
        public string UserId { get; set; } = null!;

        
        public string? DisplayName { get; set; }

        // URL към аватара
        public string? AvatarUrl { get; set; }

       
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
