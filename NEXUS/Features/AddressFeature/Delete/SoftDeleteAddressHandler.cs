using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.AddressFeature.Delete;

public sealed class SoftDeleteAddressHandler(AppDbContext _db)
{
    public async Task<Response<AddressDeletedEvent>> Handle(SoftDeleteAddressCommand command)
    {
        var address = await _db.Addresses.FindAsync(command.Id);
        if (address == null)
            return Response<AddressDeletedEvent>.Failure(Error.NotFound("Address.NotFound", "Address not found."));

        address.IsDeleted = true;
        address.DeletedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Response<AddressDeletedEvent>.Success(new AddressDeletedEvent(address.Id));
    }
}
