using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1}")]
        public string Title { get; set; }
        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        [ForeignKey("Author")]
        public int AuthorID { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Author? Author { get; set; }
    }
}
