namespace eCommerce.OrderService.BusinessLogicLayer.Consumer;

public record ProductUpdateMessage(Guid ProductID, string? NewName);