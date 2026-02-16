using NEXUS.Data.Enums;

namespace NEXUS.Features.EntityWatchlistFeature.GetList;

public record WatchlistEntryDto(
    Guid Id,
    string Keyword,
    AlertLevel AlertLevel,
    string AlertLevelName,
    string Reason,
    bool IsActive,
    DateTime CreatedAt
);
