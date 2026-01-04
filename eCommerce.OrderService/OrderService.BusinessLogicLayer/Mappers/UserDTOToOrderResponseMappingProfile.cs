using eCommerce.OrderService.BusinessLogicLayer.DTO;
using Mapster;

namespace OrderService.BusinessLogicLayer.Mappers;

public class UserDTOToOrderResponseMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserDTO, OrderResponse>()
          .Map(dest => dest.PersonName, src => src.PersonName)
          .Map(dest => dest.Email, src => src.Email);
    }
}