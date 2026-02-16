using NEXUS.Data.Enums;

namespace NEXUS.Features.CaseFeature.UpdateCase;

public record UpdateCaseCommand(
    Guid CaseId,
    string CaseFileNumber,
    string Title,
    DateOnly OpenDate,
    CaseStatus Status,
    string InvestigatingOfficer,
    PriorityLevel Priority
);

public record CaseUpdatedEvent(Guid CaseId);
