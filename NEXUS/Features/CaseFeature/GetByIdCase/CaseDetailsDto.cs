using NEXUS.Data.Enums;

namespace NEXUS.Features.CaseFeature.GetByIdCase;

public record CaseDetailsDto(
    Guid Id,
    string CaseFileNumber,
    string Title,
    DateOnly OpenDate,
    CaseStatus Status,
    string? StatusName,
    string InvestigatingOfficer,
    PriorityLevel Priority,
    string? PriorityName,
    DateTime CreatedAt,
    int SuspectsCount,
    List<CaseSuspectDto> Suspects
);

public record CaseSuspectDto(
    Guid SuspectId,
    string FullName,
    AccusationType AccusationType,
    string? AccusationTypeName,
    LegalStatusInCase LegalStatus,
    string? LegalStatusName,
    DateOnly? ConfessionDate,
    string? Notes
);
