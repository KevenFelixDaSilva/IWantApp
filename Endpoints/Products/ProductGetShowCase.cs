using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;

namespace IWantApp.Endpoints.Products;

public class ProductGetShowCase
{
    public static string Template => "/products/showcase";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(int? page, int? rows, string? orderBy,ApplicationDbContext context)
    {
        if (page == null)
            page = 1;
        if (rows == null)
            rows = 10;
        if (string.IsNullOrEmpty(orderBy))
            orderBy = "name";

        var queryBase = context.Products.Include(p => p.Category)
            .Where(p => p.HasStock && p.Category.Active);

        if(orderBy == "name")
            queryBase = queryBase.OrderBy(p => p.Name);
        else
            queryBase = queryBase.OrderBy(p => p.Price);

        var queryFilter = queryBase.Skip((page.Value - 1)* rows.Value).Take(rows.Value);
        var products = queryFilter.ToList();

        var response = products.Select(p => new ProductResponse(p.Name, p.Category , p.Description, p.HasStock, p.Price ,p.Active));

        return Results.Ok(response);
    }
}
