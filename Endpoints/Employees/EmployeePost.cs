using IWantApp.Domain.Products;
using IWantApp.Endpoints.Categories;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IWantApp.Endpoints.Employees;

public class EmployeePost
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;
    public static IResult Action(EmployeeRequest employeeResquest, UserManager<IdentityUser> userManager)
    {
        var user = new IdentityUser { UserName = employeeResquest.Name, Email = employeeResquest.Email };
        var result = userManager.CreateAsync(user).Result;

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertProblemDetails());
            
        var userClaim = new List<Claim>()
        {
            new Claim("EmployeeCode", employeeResquest.EmployeesCode),
            new Claim("Name", employeeResquest.Name)
        };

        var claimResult =
            userManager.AddClaimsAsync(user, userClaim).Result;

        if(!claimResult.Succeeded)
            return Results.BadRequest(claimResult.Errors.First());

        return Results.Created($"/employee/{user.Id}", user.Id);
    }
}
