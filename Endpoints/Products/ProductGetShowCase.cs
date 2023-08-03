﻿using Microsoft.AspNetCore.Authorization;

namespace IWantApp.Endpoints.Products;

public class ProductGetShowCase
{
    public static string Template => "/products/showcase";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(ApplicationDbContext context, int page = 1, int rows = 10, string orderBy = "name")
    {
        if (rows > 10)
            return Results.Problem(title: "rows with max 10");

        var queryBase = context.Products.AsNoTracking().Include(p => p.Category)
            .Where(p => p.HasStock && p.Category.Active);

        if (orderBy == "name")
            queryBase = queryBase.OrderBy(p => p.Name);
        else if (orderBy == "price")
            queryBase = queryBase.OrderBy(p => p.Price);
        else
            return Results.Problem(title: "Order only by price or name", statusCode: 400);


        var queryFilter = queryBase.Skip((page - 1)* rows).Take(rows);
        var products = queryFilter.ToList();

        var response = products.Select(p => new ProductResponse(p.Name, p.Category , p.Description, p.HasStock, p.Price ,p.Active));

        return Results.Ok(response);
    }
}
