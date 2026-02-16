namespace NEXUS.Features.CaseFeature.RemoveSupectFromCase;

public record RemoveSuspectFromCaseCommand(Guid CaseId, Guid SuspectId);

public record SuspectRemovedFromCaseEvent(Guid CaseId, Guid SuspectId);
