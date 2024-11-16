using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CMSAPI.Data;
using CMSAPI.Models;
using CMSAPI.Services.FolderServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CMSAPI.Services.AuthServices;

public class AuthService : IAuthService {
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _config;
    private readonly IFolderService _folderService;

    public AuthService(UserManager<IdentityUser> userManager, IConfiguration config, IFolderService folderService) {
        _userManager = userManager;
        _config = config;
        _folderService = folderService;
    }

    // Register a new user with IdentityUser
    public async Task<bool> RegisterUser(string username, string email, string password) {
        var identityUser = new IdentityUser {
            UserName = username,
            Email = email
        };


        var result = await _userManager.CreateAsync(identityUser, password);

        if (result.Succeeded) {
            var user = await _userManager.FindByEmailAsync(email);
            await _folderService.CreateRootFolder(user.Id);
        }

        return result.Succeeded;
    }

    // Login by verifying the user's email and password
    public async Task<bool> Login(string email, string password) {
        var identityUser = await _userManager.FindByEmailAsync(email);
        if (identityUser == null) {
            return false;
        }

        return await _userManager.CheckPasswordAsync(identityUser, password);
    }

    // Generate a JWT token for the authenticated IdentityUser
    public string GenerateTokenString(IdentityUser user) {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id) // IdentityUser ID
            };

        var jwtKey = _config.GetSection("Jwt:Key").Value;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(48),
            issuer: _config.GetSection("Jwt:Issuer").Value,
            audience: _config.GetSection("Jwt:Audience").Value,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
