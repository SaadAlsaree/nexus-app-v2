using NEXUS.Data.Enums;

namespace NEXUS.Features.OrgUnitFeature.GetList;

public record OrgUnitSummaryDto(
    Guid Id,
    string UnitName,
    OrgUnitLevel Level,
    string LevelName,
    string Path,
    int SubUnitsCount,
    DateTime CreatedAt
);

public record GetOrgUnitsQuery(
    string? SearchTerm = null,
    OrgUnitLevel? Level = null,
    int Page = 1,
    int PageSize = 10
);
