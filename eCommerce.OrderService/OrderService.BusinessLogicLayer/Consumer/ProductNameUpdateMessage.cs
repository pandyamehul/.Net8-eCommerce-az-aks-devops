namespace eCommerce.OrderService.BusinessLogicLayer.Consumer;

public record ProductNameUpdateMessage(Guid ProductID, string? NewName);