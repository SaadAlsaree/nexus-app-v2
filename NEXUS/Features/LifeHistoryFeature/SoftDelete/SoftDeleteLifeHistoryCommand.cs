namespace NEXUS.Features.LifeHistoryFeature.SoftDelete;

public record SoftDeleteLifeHistoryCommand(Guid LifeHistoryId);

public record LifeHistoryDeletedEvent(Guid LifeHistoryId);
