using NEXUS.Data.Enums;

namespace NEXUS.Features.AddressFeature.CreateSuspectAddres;

public record CreateSuspectAddressCommand(
    Guid SuspectId,
    AddressType Type,
    string City,
    string District,
    string? Details,
    string? GPSCoordinates
);

public record SuspectAddressCreatedEvent(
    Guid AddressId,
    Guid SuspectId,
    AddressType Type,
    string City,
    string District
);
