namespace IWantApp.Endpoints.Products;

public record ProductResponse(Guid Id ,string Name, Category Category, string Description, bool HasStock, decimal price, bool Active);
