using NEXUS.Data.Enums;

namespace NEXUS.Features.InterrogationSessionFeature.GetList;

public record InterrogationSessionSummaryDto(
    Guid Id,
    Guid SuspectId,
    string SuspectFullName,
    Guid CaseId,
    string CaseTitle,
    DateOnly SessionDate,
    string InterrogatorName,
    string Location,
    InterrogationType SessionType,
    string? SessionTypeName,
    InterrogationOutcome Outcome,
    string? OutcomeName,
    bool IsRatifiedByJudge,
    DateTime CreatedAt
);
