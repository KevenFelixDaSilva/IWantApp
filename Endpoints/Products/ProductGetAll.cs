using IWantApp.Endpoints.Products;
using Microsoft.AspNetCore.Authorization;

namespace IWantApp.Endpoints.Products;

[Authorize(Policy = "EmploeeyPolicy")]
public class ProductGetAll
{
    public static string Template => "/products";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;
    public static async Task<IResult> Action(ApplicationDbContext context)
    {
        var products = context.Products.Include(p => p.Category).OrderBy(p => p.Name).ToList();
        var response = products.Select(p => new ProductResponse(p.Name, p.Category , p.Description, p.HasStock, p.Price,p.Active));

        return Results.Ok(response);
    }
}
