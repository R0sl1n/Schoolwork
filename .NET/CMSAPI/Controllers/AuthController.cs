using System.Threading.Tasks;
using CMSAPI.DTOs;
using CMSAPI.Services.AuthServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CMSAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    // UserManager handles IdentityUsers, AuthService for authentication
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService, UserManager<IdentityUser> userManager)
    {
        _authService = authService;
        _userManager = userManager;
    }
    
    [HttpPost("Register")]
    public async Task<IActionResult> Register(CreateUserDto user)
    {
        var identityUser = await _userManager.FindByEmailAsync(user.Email);
        // return 409 conflict if user already exists
        if (identityUser != null)
        {
            return Conflict(new
            {
                IsSuccess = false,
                Message = "Account already exists! Login in stead."
            });
        }
        
        // Try to register user and return 200 Ok response if success
        var result = await _authService.RegisterUser(user.Username, user.Email, user.Password);
        if (result)
        {
            return Ok(new
            {
                IsSuccess = true,
                Message = "User registered successfully!"
            });
        }
        
        // Else 400 BadRequest response
        return BadRequest(new
        {
            IsSuccess = false,
            Message = "User registration failed. Please try again."
        });
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto user)
    {
        // Check if a user with the specified email exists
        var identityUser = await _userManager.FindByEmailAsync(user.Email);
        // Return 400 BadRequest if the user does not exist
        if (identityUser == null)
        {
            return BadRequest(new
            {
                IsSuccess = false,
                Message = "User does not exist. Please register first."
            });
        }
        
        // Validate the model
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                IsSuccess = false,
                Message = "Invalid input. Please check your data and try again."
            });
        }
        
        // Check if login credentials are correct via AuthService
        // 400 Bad request response if fails
        if (!await _authService.Login(user.Email, user.Password)) 
        {
            return BadRequest(new
            {
                IsSuccess = false,
                Message = "Something went wrong when logging in..."
            });
        }
        
        // If login success: return 200 Ok response, and generate token
        return Ok(new
        {
            IsSuccess = true,
            Token = _authService.GenerateTokenString(identityUser),
            Message = "Login Successful!"
        });
    }
}