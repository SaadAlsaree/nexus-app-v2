namespace NEXUS.Features.CaseFeature.SoftDeleteCase;

public record SoftDeleteCaseCommand(Guid CaseId);

public record CaseDeletedEvent(Guid CaseId);
