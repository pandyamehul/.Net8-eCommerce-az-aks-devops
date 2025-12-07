using eCommerce.ProductsService.BusinessLogicLayer.DTO;
using eCommerce.ProductsService.DataAccessLayer.Entities;
using Mapster;

namespace eCommerce.ProductService.BusinessAccessLayer.Mappers;

public static class ProductMappingProfile
{
    public static void RegisterMappings()
    {
        // ProductAddRequest to Product
        TypeAdapterConfig<ProductAddRequest, Product>
            .NewConfig()
            .Map(dest => dest.ProductName, src => src.ProductName)
            .Map(dest => dest.Category, src => src.Category.ToString())
            .Map(dest => dest.UnitPrice, src => src.UnitPrice)
            .Map(dest => dest.QuantityInStock, src => src.QuantityInStock)
            .Ignore(dest => dest.ProductID);
        ;

        // ProductAddRequest to Product
        TypeAdapterConfig<Product, ProductResponse>
            .NewConfig()
            .Map(dest => dest.ProductName, src => src.ProductName)
            .Map(dest => dest.Category, src => src.Category)
            .Map(dest => dest.UnitPrice, src => src.UnitPrice)
            .Map(dest => dest.QuantityInStock, src => src.QuantityInStock)
            .Map(dest => dest.ProductID, src => src.ProductID)
        ;

        // ProductAddRequest to Product
        TypeAdapterConfig<ProductUpdateRequest, Product>
            .NewConfig()
            .Map(dest => dest.ProductName, src => src.ProductName)
            .Map(dest => dest.Category, src => src.Category)
            .Map(dest => dest.UnitPrice, src => src.UnitPrice)
            .Map(dest => dest.QuantityInStock, src => src.QuantityInStock)
            .Map(dest => dest.ProductID, src => src.ProductID)
        ;
    }
}