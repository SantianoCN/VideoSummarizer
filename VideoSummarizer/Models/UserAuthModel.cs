using System.ComponentModel.DataAnnotations;

namespace VideoSummarizer.Models
{
    public class UserAuthModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Range(12, 120, ErrorMessage = "Age must be between 12 and 120")]
        public int Age { get; set; }
    }
}