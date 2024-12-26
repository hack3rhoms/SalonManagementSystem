using System.ComponentModel.DataAnnotations;

namespace SalonManagementSystem.Models
{
    public class Login
    {
        
        [Required]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please enter a Email!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter a Password!")]
        public string Password { get; set; }
    }
}
