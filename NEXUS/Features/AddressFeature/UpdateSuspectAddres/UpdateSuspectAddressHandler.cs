using MapsterMapper;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.AddressFeature.UpdateSuspectAddres;

public sealed class UpdateSuspectAddressHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<SuspectAddressUpdatedEvent>> Handle(UpdateSuspectAddressCommand command)
    {
        var address = await _db.Addresses.FindAsync(command.AddressId);
        if (address == null)
            return Response<SuspectAddressUpdatedEvent>.Failure(Error.NotFound("Address.NotFound", "Address not found."));

        _mapper.Map(command, address);
        address.LastUpdateAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Response<SuspectAddressUpdatedEvent>.Success(new SuspectAddressUpdatedEvent(address.Id));
    }
}
