using NEXUS.Data.Enums;

namespace NEXUS.Features.OrgUnitFeature.Create;

public record CreateOrgUnitCommand(
    string UnitName,
    OrgUnitLevel Level,
    Guid? ParentUnitId
);

public record OrgUnitCreatedEvent(Guid OrgUnitId, string UnitName, string Path);
