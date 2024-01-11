using System.ComponentModel.DataAnnotations;

namespace CRUD.Models
{
    public class SignIn
    {
        [Required, EmailAddress]
        public String? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
