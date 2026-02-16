namespace NEXUS.Features.OrgAssignmentFeature.Remove;

public record RemoveOrgAssignmentCommand(Guid Id);

public record OrgAssignmentRemovedEvent(Guid AssignmentId);
