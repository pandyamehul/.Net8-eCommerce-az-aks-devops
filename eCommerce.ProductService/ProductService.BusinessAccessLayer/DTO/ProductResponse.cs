namespace eCommerce.ProductsService.BusinessLogicLayer.DTO;

public record ProductResponse(Guid ProductID, string ProductName, CategoryOptions Category, double? UnitPrice, int? QuantityInStock)
{
    public ProductResponse() : this(default, string.Empty, default, default, default)
    {

    }
}