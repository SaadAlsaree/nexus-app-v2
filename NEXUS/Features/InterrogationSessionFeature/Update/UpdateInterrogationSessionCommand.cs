using NEXUS.Data.Enums;

namespace NEXUS.Features.InterrogationSessionFeature.Update;

public record UpdateInterrogationSessionCommand(
    Guid InterrogationSessionId,
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

public record InterrogationSessionUpdatedEvent(Guid InterrogationSessionId);
