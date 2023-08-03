using IWantApp.Domain.Products;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IWantApp.Endpoints.Categories;

public class CategoryPost
{
    public static string Template => "/category";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;
    
    [Authorize]
    public static async Task<IResult> Action(CategoryRequest categoryRequest, HttpContext http,ApplicationDbContext Context)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var category = new Category(categoryRequest.Name, userId, userId);
       
        if (!category.IsValid)
        {
            return Results.ValidationProblem(category.Notifications.ConvertProblemDetails());
        }
            
        await Context.Categories.AddAsync(category);
        await Context.SaveChangesAsync();
        
        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}
