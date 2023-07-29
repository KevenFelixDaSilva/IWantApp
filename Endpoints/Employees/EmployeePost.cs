using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace IWantApp.Endpoints.Employees;

public class EmployeePost
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;
    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(EmployeeRequest employeeResquest,HttpContext http, UserManager<IdentityUser> userManager)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var newUser = new IdentityUser { UserName = employeeResquest.Name, Email = employeeResquest.Email };
        var result = await userManager.CreateAsync(newUser);

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertProblemDetails());
            
        var userClaim = new List<Claim>()
        {
            new Claim("EmployeeCode", employeeResquest.EmployeesCode),
            new Claim("Name", employeeResquest.Name),
            new Claim("CreatedBy", userId),
        };

        var claimResult = await userManager.AddClaimsAsync(newUser, userClaim);

        if(!claimResult.Succeeded)
            return Results.BadRequest(claimResult.Errors.First());

        return Results.Created($"/employee/{newUser.Id}", newUser.Id);
    }
}
