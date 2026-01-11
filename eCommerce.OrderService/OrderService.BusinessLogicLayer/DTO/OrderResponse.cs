namespace eCommerce.OrderService.BusinessLogicLayer.DTO;

public record OrderResponse(
    Guid OrderID,
    Guid UserID,
    decimal TotalBill,
    DateTime OrderDate,
    List<OrderItemResponse> OrderItems,
    string? PersonName,
    string? Email
)
{
    public OrderResponse() : this(default, default, default, default, new List<OrderItemResponse>(), null, null)
    {
    }
}
