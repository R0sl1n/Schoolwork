using System.ComponentModel.DataAnnotations;

namespace CMSAPI.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
    }

    public class CreateUserDto
    {
        [Required(ErrorMessage = "Username required!")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password required!")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Email required!")]
        public string? Email { get; set; }
    }

    public class UpdateUserDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}