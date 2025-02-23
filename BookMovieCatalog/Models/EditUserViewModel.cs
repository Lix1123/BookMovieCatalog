using System.ComponentModel.DataAnnotations;

namespace BookMovieCatalog.Models
{
    public class EditUserViewModel
    {
        [Required]
        public string Id { get; set; } = string.Empty; 

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty; 

        [Phone]
        public string PhoneNumber { get; set; } = string.Empty; 
    }
}
