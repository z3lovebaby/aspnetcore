using System.ComponentModel.DataAnnotations;

namespace CRUD.Models
{
    public class SignUp
    {
        public String? FirstName { get; set; }
        public String? LastName { get; set; }
        [Required,EmailAddress]
        public String? Email { get; set; }
        [Required]
        public String? PassWord { get; set; }
        [Required]
        public String? RePassWord { get; set; }
    }
}
