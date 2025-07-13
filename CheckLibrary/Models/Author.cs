using System.ComponentModel.DataAnnotations;

namespace CheckLibrary.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} required")]
        [StringLength(60, MinimumLength = 3, ErrorMessage ="{0} size should be between {2} and {1}")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} required")]
        public DateOnly BirthDay { get; set; }
        public string Nationality { get; set; } //Buscar de uma api do .gov os países no momento de cadastrar
        public ICollection<Book> Books { get; set; }
    }
}
