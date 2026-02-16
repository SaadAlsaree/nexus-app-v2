using NEXUS.Data.Enums;

namespace NEXUS.Features.OrgUnitFeature.GetHierarchy;

public record OrgUnitHierarchyDto(
    Guid Id,
    string UnitName,
    OrgUnitLevel Level,
    string LevelName,
    string Path,
    List<OrgUnitHierarchyDto> SubUnits
);

public record GetOrgUnitHierarchyQuery();
