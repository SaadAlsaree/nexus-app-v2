using NEXUS.Data.Enums;

namespace NEXUS.Features.OrgUnitFeature.GetById;

public record OrgUnitDetailsDto(
    Guid Id,
    string UnitName,
    OrgUnitLevel Level,
    string LevelName,
    Guid? ParentUnitId,
    string? ParentUnitName,
    string Path,
    DateTime CreatedAt
);

public record GetOrgUnitByIdQuery(Guid Id);
