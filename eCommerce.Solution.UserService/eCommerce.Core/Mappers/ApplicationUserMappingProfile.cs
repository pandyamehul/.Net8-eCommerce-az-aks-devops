using Mapster;
using eCommerce.Core.DTO;
using eCommerce.Core.Entities;

namespace eCommerce.Core.Mappers;

public static class MappingConfig
{
    public static void RegisterMappings()
    {
        // RegisterRequest to ApplicationUser
        TypeAdapterConfig<RegisterRequest, ApplicationUser>
            .NewConfig()
            .Map(dest => dest.Gender, src => src.Gender.ToString())
            .Ignore(dest => dest.UserID);

        // ApplicationUser to AuthenticationResponse
        TypeAdapterConfig<ApplicationUser, AuthenticationResponse>
            .NewConfig()
            .Map(dest => dest.UserID, src => src.UserID)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.PersonName, src => src.PersonName)
            .Map(dest => dest.Gender, src => src.Gender)
            .Ignore(dest => dest.Success);
        // .Ignore(dest => dest.Token);
    }
}
