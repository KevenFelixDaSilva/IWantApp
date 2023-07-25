using IWantApp.Domain.Products;
using IWantApp.Infra.Data;

namespace IWantApp.Endpoints.Categories;

public class CategoryGetAll
{
    public static string Template => "/Categories";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;
    public static IResult Action(ApplicationDbContext Context)
    {
        var categories = Context.Categories.ToList();
        var response = categories.Select(c => new CategoryResponse { Id = c.Id, Name =  c.Name, Active = c.Active });

        return Results.Ok(response);
    }
}
