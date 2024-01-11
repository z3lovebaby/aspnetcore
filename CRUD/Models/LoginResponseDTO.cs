namespace CRUD.Models
{
    public class LoginResponseDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string ReToken { get; set; }
        public List<string> Roles { get; set; }
    }
}
