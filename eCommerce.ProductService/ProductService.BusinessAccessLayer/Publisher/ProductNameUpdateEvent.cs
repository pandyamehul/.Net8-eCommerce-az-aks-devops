namespace eCommerce.ProductService.BusinessAccessLayer.Publisher;

public record ProductNameUpdateEvent(Guid ProductID, string? NewName);