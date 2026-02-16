using Mapster;
using NEXUS.Data.Entities;
using NEXUS.Features.OrgAssignmentFeature.Add;

namespace NEXUS.Mappings;

public class OrgAssignmentMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddOrgAssignmentCommand, Assignment>()
            .Map(dest => dest.Id, src => Guid.NewGuid())
            .Map(dest => dest.CreatedAt, src => DateTime.UtcNow);
    }
}
