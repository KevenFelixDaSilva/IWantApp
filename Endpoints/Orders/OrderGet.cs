using IWantApp.Domain.Orders;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IWantApp.Endpoints.Orders;

public class OrderGet
{
    public static string Template => "/orders/{id}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [Authorize]
    public static async Task<IResult> Action(ApplicationDbContext context, HttpContext http, Guid id, UserManager<IdentityUser> userManager)
    {
        var clientClaim = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
        var employeeClaim = http.User.Claims.FirstOrDefault(c => c.Type == "EmployCode");
        
        var order = context.Orders.Include(o => o.Products).FirstOrDefault(p => p.Id == id);

        if (order.ClientId != clientClaim.Value && employeeClaim.Value == null)
            return Results.Forbid();

        var client = await userManager.FindByIdAsync(order.ClientId);

        var productResponse = context.Products.Select(p => new OrderProduct (p.Id, p.Name ));
        var orderResponse = new OrderResponse(order.Id, client.UserName, productResponse, order.DeliveryAddress);

        return Results.Ok(orderResponse);
    }
}
