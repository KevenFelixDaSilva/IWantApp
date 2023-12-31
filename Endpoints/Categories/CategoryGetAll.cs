﻿namespace IWantApp.Endpoints.Categories;

public class CategoryGetAll
{
    public static string Template => "/category";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;
    public static async Task<IResult> Action(ApplicationDbContext Context)
    {
        var categories = await Context.Categories.ToListAsync();
        var response = categories.Select(c => new CategoryResponse { Id = c.Id, Name =  c.Name, Active = c.Active });

        return Results.Ok(response);
    }
}
