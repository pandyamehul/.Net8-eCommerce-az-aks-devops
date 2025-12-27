
using eCommerce.OrderService.BusinessLogicLayer.DTO;
using eCommerce.OrderService.DataAccessLayer.Entities;
using Mapster;

namespace eCommerce.OrderService.BusinessLogicLayer.Mappers;

public class OrderToOrderResponseMappingProfile
{
    public OrderToOrderResponseMappingProfile()
    {
        TypeAdapterConfig.GlobalSettings.NewConfig<Order, OrderResponse>()
            .Map(dest => dest.OrderID, src => src.OrderID)
            .Map(dest => dest.UserID, src => src.UserID)
            .Map(dest => dest.OrderDate, src => src.OrderDate)
            .Map(dest => dest.TotalBill, src => src.TotalBill)
            .Map(dest => dest.OrderItems, src => src.OrderItems);
    }
}
