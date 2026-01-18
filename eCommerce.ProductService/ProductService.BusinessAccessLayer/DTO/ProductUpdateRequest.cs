namespace eCommerce.ProductsService.BusinessLogicLayer.DTO;

public record ProductUpdateRequest(Guid ProductID, string ProductName, CategoryOptions Category, double? UnitPrice, int? QuantityInStock)
{
    public ProductUpdateRequest() : this(default, string.Empty, default, default, default)
    {

    }
}