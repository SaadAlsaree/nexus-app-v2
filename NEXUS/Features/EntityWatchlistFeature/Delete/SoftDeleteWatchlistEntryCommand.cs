namespace NEXUS.Features.EntityWatchlistFeature.Delete;

public record SoftDeleteWatchlistEntryCommand(Guid Id);

public record WatchlistEntryDeletedEvent(Guid Id);
