namespace NEXUS.Features.LifeHistoryFeature.Update;

public record UpdateLifeHistoryCommand(
    Guid LifeHistoryId,
    string? EducationLevel,
    string? SchoolsAttended,
    string? CivilianJobs,
    string? RadicalizationStory
);

public record LifeHistoryUpdatedEvent(Guid LifeHistoryId);
