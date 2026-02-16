using NEXUS.Data.Enums;

namespace NEXUS.Features.AddressFeature.UpdateSuspectAddres;

public record UpdateSuspectAddressCommand(
    Guid AddressId,
    AddressType Type,
    string City,
    string District,
    string? Details,
    string? GPSCoordinates
);

public record SuspectAddressUpdatedEvent(Guid AddressId);
