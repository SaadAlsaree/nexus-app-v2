using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.AddressFeature.GetByIdSuspectAddres;

public sealed class GetByIdSuspectAddressHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<AddressDto>> Handle(GetByIdSuspectAddressQuery query)
    {
        var address = await _db.Addresses
            .AsNoTracking()
            .Where(a => a.Id == query.Id && a.IsDeleted == false)
            .FirstOrDefaultAsync();

        if (address == null)
            return Response<AddressDto>.Failure(Error.NotFound("Address.NotFound", "Address not found."));

        var dto = _mapper.Map<AddressDto>(address);

        return Response<AddressDto>.Success(dto);
    }
}
