using System.ComponentModel.DataAnnotations;

namespace CheckLibrary.Models
{
    public class Library
    {
        public int Id { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1}")]
        public string Title { get; set; }
        [Required]
        public string ListBooks { get; set; }
    }
}