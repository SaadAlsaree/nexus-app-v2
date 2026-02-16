using NEXUS.Data.Enums;

namespace NEXUS.Features.SuspectFeature.UpdateStatus;

public record UpdateSuspectStatusCommand(Guid SuspectId, SuspectStatus NewStatus, string? Notes);

public record SuspectStatusUpdatedEvent(Guid SuspectId, SuspectStatus OldStatus, SuspectStatus NewStatus);
