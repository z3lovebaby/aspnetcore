using System.ComponentModel.DataAnnotations;
namespace CRUD.Models
{
    public class UserRefreshTokens
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? RefreshToken { get; set; }
        public DateTime ExpirationTime { get; set; }

        public bool IsActive { get; set; } = true;
    }
}