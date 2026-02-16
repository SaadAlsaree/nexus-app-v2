using Mapster;
using NEXUS.Data.Entities;
using NEXUS.Extensions;
using NEXUS.Features.EntityWatchlistFeature.GetList;
using NEXUS.Features.EntityWatchlistFeature.Update;

namespace NEXUS.Mappings;

public class EntityWatchlistMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateWatchlistEntryCommand, EntityWatchlist>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatedAt);

        config.NewConfig<EntityWatchlist, WatchlistEntryDto>()
            .Map(dest => dest.AlertLevelName, src => src.AlertLevel.GetDisplayName());
    }
}
