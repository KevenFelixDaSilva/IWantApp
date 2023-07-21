using IWantApp.Domain.Products;
using IWantApp.Infra.Data;

namespace IWantApp.Endpoints.Categories;

public class CategoryPost
{
    public static string Template => "/Categories";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;
    public static IResult Action(CategoryRequest categoryRequest, ApplicationDbContext Context)
    {
        var category = new Category
        {
            Name = categoryRequest.Name,
            CreatedBy = "Test",
            CreatedOn = DateTime.Now,
            EditedBy = "Test",
            EditedOn = DateTime.Now,
            Active = true,
        };
        Context.Categories.Add(category);
        Context.SaveChanges();
        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}
