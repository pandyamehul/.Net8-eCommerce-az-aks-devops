using eCommerce.OrderService.BusinessLogicLayer.DTO;
using eCommerce.OrderService.DataAccessLayer.Entities;
using Mapster;

namespace eCommerce.OrderService.BusinessLogicLayer.Mappers;

public class OrderAddRequestToOrderMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<OrderAddRequest, Order>()
            .Map(dest => dest.UserID, src => src.UserID)
            .Map(dest => dest.OrderDate, src => src.OrderDate)
            .Map(dest => dest.OrderItems, src => src.OrderItems)
            .Ignore(dest => dest.OrderID)
            .Ignore(dest => dest._id)
            .Ignore(dest => dest.TotalBill);
    }
}
