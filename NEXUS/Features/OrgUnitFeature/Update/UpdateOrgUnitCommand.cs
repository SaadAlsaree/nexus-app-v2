using NEXUS.Data.Enums;

namespace NEXUS.Features.OrgUnitFeature.Update;

public record UpdateOrgUnitCommand(
    Guid Id,
    string UnitName,
    OrgUnitLevel Level,
    Guid? ParentUnitId
);

public record OrgUnitUpdatedEvent(Guid OrgUnitId);
