using Mapster;
using NEXUS.Data.Entities;
using NEXUS.Features.LifeHistoryFeature.Update;

namespace NEXUS.Mappings;

public class LifeHistoryMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateLifeHistoryCommand, LifeHistory>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatedAt);
    }
}
