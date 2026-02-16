using NEXUS.Data.Enums;

namespace NEXUS.Features.InterrogationSessionFeature.Create;

public record CreateInterrogationSessionCommand(
    Guid SuspectId,
    Guid CaseId,
    DateOnly SessionDate,
    string InterrogatorName,
    string Location,
    InterrogationType SessionType,
    string Content,
    string SummaryContent,
    InterrogationOutcome Outcome,
    string? InvestigatorNotes,
    bool IsRatifiedByJudge
);

public record InterrogationSessionCreatedEvent(Guid InterrogationSessionId, Guid SuspectId);
