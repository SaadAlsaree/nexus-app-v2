using NEXUS.Data.Enums;

namespace NEXUS.Features.CaseFeature.GetListCases;

public record CaseSummaryDto(
    Guid Id,
    string CaseFileNumber,
    string Title,
    DateOnly OpenDate,
    CaseStatus Status,
    string? StatusName,
    string? InvestigatingOfficer,
    PriorityLevel Priority,
    string? PriorityName,
    int SuspectsCount,
    DateTime CreatedAt
);
