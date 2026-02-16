using Mapster;
using NEXUS.Data.Entities;
using NEXUS.Extensions;
using NEXUS.Features.ContactFeature.GetByIdSuspectContact;
using NEXUS.Features.ContactFeature.UpdateSuspectContact;

namespace NEXUS.Mappings;

public class ContactMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Contact, ContactDto>()
            .Map(dest => dest.TypeName, src => src.Type.GetDisplayName());

        config.NewConfig<UpdateSuspectContactCommand, Contact>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatedAt);
    }
}
