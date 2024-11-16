using System.Threading.Tasks;
using CMSAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace CMSAPI.Services.AuthServices;

public interface IAuthService
{
    Task<bool> RegisterUser(string username, string email, string password);
    Task<bool> Login(string email, string password);
    string GenerateTokenString(IdentityUser user);
}