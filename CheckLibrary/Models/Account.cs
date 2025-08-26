using System.ComponentModel.DataAnnotations;

namespace CheckLibrary.Models
{
    public class Account
    {
        [Key]
        [Required(ErrorMessage = "{0} required")]
        [StringLength(60, MinimumLength = 10, ErrorMessage = "{0} size should be between {2} and {1}")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0} required")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "{0} size should be between {2} and {1}")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "{0} required")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "{0} size should be between {2} and {1}")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "{0} required")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1}")]
        public string FullName { get; set; }
    }
}