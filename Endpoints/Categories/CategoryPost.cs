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
        var category = new Category(categoryRequest.Name, "test", "test");
       
        if (!category.IsValid)
        {
            return Results.ValidationProblem(category.Notifications.ConvertProblemDetails());
        }
            

        Context.Categories.Add(category);
        Context.SaveChanges();
        
        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}
