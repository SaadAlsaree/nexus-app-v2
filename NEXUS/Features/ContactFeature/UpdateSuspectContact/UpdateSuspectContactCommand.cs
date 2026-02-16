using NEXUS.Data.Enums;

namespace NEXUS.Features.ContactFeature.UpdateSuspectContact;

public record UpdateSuspectContactCommand(
    Guid ContactId,
    ContactType Type,
    string Value,
    string? OwnerName
);

public record SuspectContactUpdatedEvent(Guid ContactId);
