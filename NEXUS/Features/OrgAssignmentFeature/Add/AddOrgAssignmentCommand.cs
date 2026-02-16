using NEXUS.Data.Enums;

namespace NEXUS.Features.OrgAssignmentFeature.Add;

public record AddOrgAssignmentCommand(
    Guid SuspectId,
    Guid OrgUnitId,
    Guid? DirectCommanderId,
    OrgRole RoleTitle,
    DateOnly StartDate,
    DateOnly? EndDate
);

public record OrgAssignmentAddedEvent(Guid AssignmentId, Guid SuspectId, Guid OrgUnitId);
