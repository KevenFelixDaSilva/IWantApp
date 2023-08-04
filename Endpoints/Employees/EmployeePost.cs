using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IWantApp.Endpoints.Employees;

public class EmployeePost
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;
    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(EmployeeRequest employeeResquest,HttpContext http, UserCreator userCreator)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var userClaim = new List<Claim>()
        {
            new Claim("EmployeeCode", employeeResquest.EmployeesCode),
            new Claim("Name", employeeResquest.Name),
            new Claim("CreatedBy", userId),
        };

        (IdentityResult identity, string userId) result =
            await userCreator.Create(employeeResquest.Email, employeeResquest.Passaword, userClaim);

        if (!result.identity.Succeeded)
            return Results.ValidationProblem(result.identity.Errors.ConvertProblemDetails());

            return Results.Created($"/employee/{result.userId}", result.userId);
    }
}
