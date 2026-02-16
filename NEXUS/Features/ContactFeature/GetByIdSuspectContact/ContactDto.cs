using NEXUS.Data.Enums;

namespace NEXUS.Features.ContactFeature.GetByIdSuspectContact;

public record ContactDto(
    Guid Id,
    Guid SuspectId,
    ContactType Type,
    string TypeName,
    string Value,
    string? OwnerName,
    DateTime CreatedAt
);
