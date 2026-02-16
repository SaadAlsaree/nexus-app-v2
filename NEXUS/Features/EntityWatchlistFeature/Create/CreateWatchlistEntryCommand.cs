using NEXUS.Data.Enums;

namespace NEXUS.Features.EntityWatchlistFeature.Create;

public record CreateWatchlistEntryCommand(
    string Keyword,
    AlertLevel AlertLevel,
    string Reason
);

public record WatchlistEntryCreatedEvent(Guid EntryId, string Keyword);
