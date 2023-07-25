using IWantApp.Domain.Products;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Mvc;
using System;

namespace IWantApp.Endpoints.Categories;

public class CategoryPut
{
    public static string Template => "/Categories/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handler => Action;
    public static IResult Action([FromRoute] Guid id,CategoryRequest categoryRequest, ApplicationDbContext Context)
    {
        var category = Context.Categories.Where(c => c.Id == id).FirstOrDefault();
        if (category == null) 
            return Results.NotFound();

        category.EditInfo(categoryRequest.Name, categoryRequest.Active);

        if (!category.IsValid)
            return Results.ValidationProblem(category.Notifications.ConvertProblemDetails());

        Context.SaveChanges();
        return Results.Ok();
    }
}
