namespace IWantApp.Endpoints.Products;

public record ProductResponse(string Name, Category Category, string Description, bool HasStock, decimal price, bool Active);
