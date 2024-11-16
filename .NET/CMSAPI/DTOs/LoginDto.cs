using System.ComponentModel.DataAnnotations;

namespace CMSAPI.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "Email required!")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Password required!")]
    public string Password { get; set; } = string.Empty;
}