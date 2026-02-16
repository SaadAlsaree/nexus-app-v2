namespace NEXUS.Features.AddressFeature.Delete;

public record SoftDeleteAddressCommand(Guid Id);

public record AddressDeletedEvent(Guid Id);
