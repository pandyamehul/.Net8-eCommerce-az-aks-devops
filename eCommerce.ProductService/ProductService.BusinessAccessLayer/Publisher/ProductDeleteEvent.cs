namespace eCommerce.ProductService.BusinessAccessLayer.Publisher;

public record ProductDeletionEvent(Guid ProductID, string? ProductName);