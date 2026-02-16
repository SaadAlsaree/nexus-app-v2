using Mapster;
using NEXUS.Data.Entities;
using NEXUS.Extensions;
using NEXUS.Features.OrgUnitFeature.GetById;
using NEXUS.Features.OrgUnitFeature.Update;

namespace NEXUS.Mappings;

public class OrgUnitMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<OrgUnit, OrgUnitDetailsDto>()
            .Map(dest => dest.LevelName, src => src.Level.GetDisplayName())
            .Map(dest => dest.ParentUnitName, src => src.ParentUnit != null ? src.ParentUnit.UnitName : null);

        config.NewConfig<UpdateOrgUnitCommand, OrgUnit>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatedAt)
            .Ignore(dest => dest.Path);
    }
}
