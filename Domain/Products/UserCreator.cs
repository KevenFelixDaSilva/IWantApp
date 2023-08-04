using System.Security.Claims;
namespace IWantApp.Domain.Products;

public class UserCreator
{
    private readonly UserManager<IdentityUser> _userManager;
    
    public UserCreator(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<(IdentityResult, string)> Create(string Email, string Password, List<Claim> claims)
    {
        var newUser = new IdentityUser { UserName = Email, Email = Email };
        var result = await _userManager.CreateAsync(newUser, Password);

        if (!result.Succeeded)
            return (result, string.Empty);

        return (await _userManager.AddClaimsAsync(newUser, claims), newUser.Id);
    }
}
