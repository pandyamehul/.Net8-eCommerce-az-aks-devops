
using eCommerce.OrderService.BusinessLogicLayer.DTO;
using eCommerce.OrderService.DataAccessLayer.Entities;
using Mapster;

namespace OrderService.BusinessLogicLayer.Mappers;

public class OrderUpdateRequestToOrderMappingProfile
{
    public OrderUpdateRequestToOrderMappingProfile()
    {
        TypeAdapterConfig.GlobalSettings.NewConfig<OrderUpdateRequest, Order>()
            .Map(dest => dest.OrderID, src => src.OrderID)
            .Map(dest => dest.UserID, src => src.UserID)
            .Map(dest => dest.OrderDate, src => src.OrderDate)
            .Map(dest => dest.OrderItems, src => src.OrderItems)
            .Ignore(dest => dest._id)
            .Ignore(dest => dest.TotalBill);
    }
}
