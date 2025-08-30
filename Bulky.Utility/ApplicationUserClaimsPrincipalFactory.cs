using System.Security.Claims;
using Book.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<IdentityUser, IdentityRole>
{
    private readonly UserManager<IdentityUser> _userManager;

    public ApplicationUserClaimsPrincipalFactory(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    {
        _userManager = userManager;
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(IdentityUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        var name = (user as ApplicationUser)?.Name ?? "Guest";
        identity.AddClaim(new Claim("Name", name));

        return identity;
    }
}
