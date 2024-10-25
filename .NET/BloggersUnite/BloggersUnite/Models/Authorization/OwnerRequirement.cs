using Microsoft.AspNetCore.Authorization;

public class OwnerRequirement : IAuthorizationRequirement
{
    public OwnerRequirement() { }
}