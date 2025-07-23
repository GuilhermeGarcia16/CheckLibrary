using System.ComponentModel.DataAnnotations;

namespace CheckLibrary.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} required")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1}")]
        public string Description { get; set; }
        public virtual ICollection<Book>? Books { get; set; }
    }
}
