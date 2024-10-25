using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using BloggersUnite.Models;

public class BlogOwnerAuthorizationHandler : AuthorizationHandler<OwnerRequirement, Blog>
{
    private readonly UserManager<IdentityUser> _userManager;

    public BlogOwnerAuthorizationHandler(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerRequirement requirement, Blog blog)
    {
        if (context.User == null || blog == null)
        {
            return Task.CompletedTask;
        }

        // Sjekk om den innloggede brukeren er eier av bloggen
        if (blog.OwnerId == _userManager.GetUserId(context.User))
        {
            context.Succeed(requirement); // Brukeren er eier, autorisering lykkes
        }

        return Task.CompletedTask;
    }
}