using NEXUS.Data.Enums;

namespace NEXUS.Features.CaseFeature.CreateCase;

public record CreateCaseCommand(
    string CaseFileNumber,
    string Title,
    DateOnly OpenDate,
    CaseStatus Status,
    string InvestigatingOfficer,
    PriorityLevel Priority
);

public record CaseCreatedEvent(Guid CaseId, string CaseFileNumber);
