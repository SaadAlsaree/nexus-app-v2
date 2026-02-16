using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Features.AddressFeature.GetByIdSuspectAddres;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.AddressFeature.GetBySuspectId;

public sealed class GetBySuspectIdHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<List<AddressDto>>> Handle(GetBySuspectIdQuery query)
    {
        var addresses = await _db.Addresses
            .AsNoTracking()
            .Where(a => a.SuspectId == query.SuspectId && a.IsDeleted == false)
            .ToListAsync();

        var dtos = _mapper.Map<List<AddressDto>>(addresses);

        return Response<List<AddressDto>>.Success(dtos);
    }
}
