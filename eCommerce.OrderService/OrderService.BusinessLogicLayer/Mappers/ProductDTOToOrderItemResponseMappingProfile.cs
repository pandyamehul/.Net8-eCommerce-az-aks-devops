using eCommerce.OrderService.BusinessLogicLayer.DTO;
using Mapster;

namespace OrderService.BusinessLogicLayer.Mappers;
public class ProductDTOToOrderItemResponseMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ProductDTO, OrderItemResponse>()
          .Map(dest => dest.ProductName, src => src.ProductName)
          .Map(dest => dest.Category, src => src.Category);
    }
}
