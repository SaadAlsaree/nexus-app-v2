namespace NEXUS.Features.LifeHistoryFeature.Create;

public record CreateLifeHistoryCommand(
    Guid SuspectId,
    string? EducationLevel,
    string? SchoolsAttended,
    string? CivilianJobs,
    string? RadicalizationStory
);

public record LifeHistoryCreatedEvent(Guid LifeHistoryId, Guid SuspectId);
