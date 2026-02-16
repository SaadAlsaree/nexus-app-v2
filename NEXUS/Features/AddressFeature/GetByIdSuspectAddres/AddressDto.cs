using NEXUS.Data.Enums;

namespace NEXUS.Features.AddressFeature.GetByIdSuspectAddres;

public record AddressDto(
    Guid Id,
    Guid SuspectId,
    AddressType Type,
    string TypeName,
    string City,
    string District,
    string? Details,
    string? GPSCoordinates,
    DateTime CreatedAt
);
