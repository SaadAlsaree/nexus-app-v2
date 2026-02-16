using Mapster;
using NEXUS.Data.Entities;
using NEXUS.Extensions;
using NEXUS.Features.AddressFeature.GetByIdSuspectAddres;
using NEXUS.Features.AddressFeature.UpdateSuspectAddres;

namespace NEXUS.Mappings;

public class AddressMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Address, AddressDto>()
            .Map(dest => dest.TypeName, src => src.Type.GetDisplayName());

        config.NewConfig<UpdateSuspectAddressCommand, Address>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatedAt);
    }
}
