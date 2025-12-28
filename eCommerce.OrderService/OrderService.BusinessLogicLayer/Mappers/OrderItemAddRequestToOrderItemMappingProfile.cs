using eCommerce.OrderService.BusinessLogicLayer.DTO;
using eCommerce.OrderService.DataAccessLayer.Entities;
using Mapster;

namespace eCommerce.OrderService.BusinessLogicLayer.Mappers;

public class OrderItemAddRequestToOrderItemMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<OrderItemAddRequest, OrderItem>()
            .Map(dest => dest.ProductID, src => src.ProductID)
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.UnitPrice, src => src.UnitPrice)
            .Ignore(dest => dest.TotalPrice)
            .Ignore(dest => dest._id);
    }
}
