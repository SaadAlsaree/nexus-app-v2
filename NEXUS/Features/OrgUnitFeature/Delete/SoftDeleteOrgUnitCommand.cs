namespace NEXUS.Features.OrgUnitFeature.Delete;

public record SoftDeleteOrgUnitCommand(Guid Id);

public record OrgUnitDeletedEvent(Guid Id);
