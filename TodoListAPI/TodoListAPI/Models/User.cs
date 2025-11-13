using Microsoft.AspNetCore.Identity;

namespace TodoListAPI.Models
{
    public class User : IdentityUser
    {
    }

    public class LoginUser
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterUser
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
