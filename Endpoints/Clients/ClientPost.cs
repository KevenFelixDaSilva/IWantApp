using IWantApp.Endpoints.Employees;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IWantApp.Endpoints.Clients;

public class ClientPost
{
    public static string Template => "/clients";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;


    [AllowAnonymous]
    public static async Task<IResult> Action(ClientRequest clientRequest, UserCreator userCreator)
    {

        
        var userClaim = new List<Claim>()
        {
            new Claim("Cpf", clientRequest.Cpf),
            new Claim("Name", clientRequest.Name),
        };

        (IdentityResult identity, string userId) result = 
            await userCreator.Create(clientRequest.Email, clientRequest.Passaword, userClaim);

        if (!result.identity.Succeeded)
            return Results.ValidationProblem(result.identity.Errors.ConvertProblemDetails());

        return Results.Created($"/clients/{result.userId}", result.userId);
    }
}
