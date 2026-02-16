using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.AddressFeature.CreateSuspectAddres;

public sealed class CreateSuspectAddressHandler(AppDbContext _db)
{
    public async Task<(Response<Guid>, SuspectAddressCreatedEvent)> Handle(CreateSuspectAddressCommand command)
    {
        var address = new Address
        {
            Id = Guid.NewGuid(),
            SuspectId = command.SuspectId,
            Type = command.Type,
            City = command.City,
            District = command.District,
            Details = command.Details,
            GPSCoordinates = command.GPSCoordinates,
            CreatedAt = DateTime.UtcNow
        };

        _db.Addresses.Add(address);
        await _db.SaveChangesAsync();

        return (Response<Guid>.Success(address.Id), new SuspectAddressCreatedEvent(
            address.Id,
            address.SuspectId,
            address.Type,
            address.City,
            address.District));
    }
}
