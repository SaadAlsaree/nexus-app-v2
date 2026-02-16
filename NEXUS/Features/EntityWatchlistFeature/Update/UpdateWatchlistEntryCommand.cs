using NEXUS.Data.Enums;

namespace NEXUS.Features.EntityWatchlistFeature.Update;

public record UpdateWatchlistEntryCommand(
    Guid Id,
    string Keyword,
    AlertLevel AlertLevel,
    string Reason,
    bool IsActive
);

public record WatchlistEntryUpdatedEvent(Guid EntryId);
