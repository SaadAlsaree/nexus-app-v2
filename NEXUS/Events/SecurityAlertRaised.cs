using NEXUS.Data.Enums;

namespace NEXUS.Events;


public record SecurityAlertRaised(
    Guid SuspectId,
    AlertLevel Level,
    string Message,
    string Source,
    string Recommendation
);
