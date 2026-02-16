using NEXUS.Data.Enums;

namespace NEXUS.Features.CaseFeature.AddSupectToCase;

public record AddSuspectToCaseCommand(
    Guid CaseId,
    Guid SuspectId,
    AccusationType AccusationType,
    LegalStatusInCase LegalStatus,
    DateOnly? ConfessionDate,
    string? Notes
);

public record SuspectAddedToCaseEvent(Guid CaseId, Guid SuspectId);
