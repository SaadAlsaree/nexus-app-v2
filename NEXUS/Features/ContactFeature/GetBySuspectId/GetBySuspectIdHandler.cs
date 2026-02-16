using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Features.ContactFeature.GetByIdSuspectContact;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.ContactFeature.GetBySuspectId;

public sealed class GetBySuspectIdHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<List<ContactDto>>> Handle(GetBySuspectIdQuery query)
    {
        var contacts = await _db.Contacts
            .AsNoTracking()
            .Where(c => c.SuspectId == query.SuspectId && !c.IsDeleted)
            .ToListAsync();

        var dtos = _mapper.Map<List<ContactDto>>(contacts);

        return Response<List<ContactDto>>.Success(dtos);
    }
}
