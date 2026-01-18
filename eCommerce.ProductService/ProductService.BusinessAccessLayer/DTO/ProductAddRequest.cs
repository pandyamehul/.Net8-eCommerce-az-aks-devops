namespace eCommerce.ProductsService.BusinessLogicLayer.DTO;

public record ProductAddRequest(string ProductName, CategoryOptions Category, double? UnitPrice, int? QuantityInStock)
{
    public ProductAddRequest() : this(string.Empty, default, default, default)
    {

    }
}