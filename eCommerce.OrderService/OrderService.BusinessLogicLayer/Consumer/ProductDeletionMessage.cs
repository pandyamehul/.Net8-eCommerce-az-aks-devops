namespace eCommerce.OrderService.BusinessLogicLayer.Consumer;

public record ProductDeletionMessage(Guid ProductID, string? ProductName);