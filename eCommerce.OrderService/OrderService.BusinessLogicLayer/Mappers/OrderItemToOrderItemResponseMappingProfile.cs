using eCommerce.OrderService.BusinessLogicLayer.DTO;
using eCommerce.OrderService.DataAccessLayer.Entities;
using Mapster;

namespace eCommerce.OrderService.BusinessLogicLayer.Mappers;

public class OrderItemToOrderItemResponseMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<OrderItem, OrderItemResponse>()
            .Map(dest => dest.ProductID, src => src.ProductID)
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.UnitPrice, src => src.UnitPrice)
            .Map(dest => dest.TotalPrice, src => src.TotalPrice);
    }
}
