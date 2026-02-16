using NEXUS.Data.Enums;

namespace NEXUS.Features.ContactFeature.CreateSuspectContact;

public record CreateSuspectContactCommand(
    Guid SuspectId,
    ContactType Type,
    string Value,
    string? OwnerName
);

public record SuspectContactCreatedEvent(
    Guid ContactId,
    Guid SuspectId,
    ContactType Type,
    string Value
);
